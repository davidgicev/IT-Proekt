function SearchAndFilter(config) {
    this.genres = []
    this.selectedActors = []
    this.html = config
    this.movies = []
    this.actorQuery = new ActorQuery(
        this.selectedActors,
        this.finishedActorQ.bind(this)
    );
    this.placeholders = {
        loading: '<div class="ring-wrapper"><div></div></div>',
        empty: '<div class="no-results">No results</div>'
    }
    this.request = null
    this.init()
}

SearchAndFilter.prototype.init = function () {
    let ref = this.html.genres
    while (ref.firstElementChild) {
        let el = ref.removeChild(ref.firstElementChild)
        let wrapper = document.createElement("div")
        wrapper.className = "genre-pill"
        let txt = document.createElement("p")
        txt.textContent = el.textContent
        let close = document.createElement("i")
        close.classList.add("fas", "fa-times")
        wrapper.appendChild(txt)
        wrapper.appendChild(close)
        wrapper.addEventListener("click", function () {
            this.genreToggle(el.dataset.id)
        }.bind(this));
        this.genres.push({
            name: el.textContent,
            id: el.dataset.id,
            html: wrapper,
            selected: false
        })
    }
    this.html.input.addEventListener("input", this.keyboardClick.bind(this))
    this.html.actorQ.addEventListener("click", this.actorQClick.bind(this))
    this.populateFilters()
}

SearchAndFilter.prototype.populateFilters = function () {
    $(this.html.genres).empty();
    $(this.html.active).empty();
    for (let actor of this.selectedActors) {
        this.html.active.appendChild(actor.html)
    }
    for (let genre of this.genres) {
        if (genre.selected)
            this.html.active.appendChild(genre.html)
        else
            this.html.genres.appendChild(genre.html)
    }
}

SearchAndFilter.prototype.finishedActorQ = function (selected) {
    if (selected == null)
        return
    let el = document.createElement("div")
    el.className = "actor-pill"
    let img = document.createElement("div")
    img.style.backgroundImage = selected.img;
    let txt = document.createElement("div")
    txt.textContent = selected.name;
    //let close = document.createElement("i")
    //close.classList.add("fas", "fa-times")
    el.addEventListener("click", function () {
        this.actorToggle(selected.id);
    }.bind(this));
    el.appendChild(img)
    el.appendChild(txt)
    //el.appendChild(close)
    let reformed = {
        id: selected.id,
        html: el,
        name: selected.name,
        img: selected.img
    }
    this.selectedActors.push(reformed)
    this.sendRequest()
    this.populateFilters()
}

SearchAndFilter.prototype.populateMovies = function () {
    let ref = this.html.result
    while (ref.firstElementChild)
        ref.removeChild(ref.firstElementChild)
    if (this.movies == null) {
        $(ref).append(this.placeholders.loading)
        return;
    }
    if (this.movies.length == 0) {
        $(ref).append(this.placeholders.empty)
        return;
    }
    for (let movie of this.movies) {
        ref.appendChild(movie)
    }
}

SearchAndFilter.prototype.unpackResults = function (data) {
    this.movies = $($.parseHTML(data)).filter(".body-content")
        .children().toArray().filter(m => m.classList.contains("movie-card"))
}

SearchAndFilter.prototype.keyboardClick = function () {
    this.sendRequest()
}

SearchAndFilter.prototype.actorQClick = function () {
    this.actorQuery.open()
}

SearchAndFilter.prototype.genreToggle = function (id) {
    let genre = this.genres.find(g => g.id == id)
    genre.selected = !genre.selected
    this.sendRequest()
    this.populateFilters()
}

SearchAndFilter.prototype.actorToggle = function (id) {
    let index = this.selectedActors.findIndex(a => a.id == id)
    this.selectedActors.splice(index, 1)
    this.sendRequest()
    this.populateFilters()
}

SearchAndFilter.prototype.sendRequest = async function () {
    this.movies = null
    this.populateMovies()
    history.replaceState({
        actors: this.selectedActors.map(a => ({
            id: a.id, name: a.name, img: a.img
        })),
        genres: this.genres.filter(x => x.selected).map(g => g.id),
        query: this.html.input.value
    }, "", "/Search")
    this.request = $.ajax({
        type: "GET",
        url: "/Search/Filter",
        data: {
            genres: this.genres.filter(g => g.selected).map(g => g.id),
            query: this.html.input.value,
            actors: this.selectedActors.map(a => a.id)
        },
        complete: function (data) {
            if (data.readyState == 4)
                this.unpackResults(data.responseText)
            else
                this.movies = null;
            this.populateMovies();
        },
        context: this,
        traditional: true
    })
}

window.addEventListener("load", function () {

    let field = document.getElementById("search");
    let ref = new SearchAndFilter({
        input: document.getElementById("search"),
        genres: document.getElementById("genres"),
        active: document.getElementById("activeFilters"),
        result: document.getElementById("result"),
        actorQ: document.getElementById("actor_search")
    })


    if (history.state) {
        let state = history.state
        ref.html.input.value = state.query || ""
        state.genres.forEach(ref.genreToggle, ref)
        state.actors.forEach(ref.finishedActorQ, ref)
        if (!state.genres.length && !state.actors.length)
            ref.sendRequest()
    }
    else {
        if (suppliedType == "True") {
            ref.genreToggle(parseInt(suppliedId))
        }
        else if (suppliedType == "False") {
            ref.finishedActorQ({
                id: suppliedId,
                name: suppliedDetails.name,
                img: 'url("https://image.tmdb.org/t/p/w185' + suppliedDetails.img + '")'
            })
        }
        else
            ref.sendRequest()
    }


    field.focus()
})