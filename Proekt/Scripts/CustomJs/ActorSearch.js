function ActorQuery(selected, callback) {
    this.callback = callback
    this.selected = selected
    this.actors = []
    this.rendered = null
    this.html = null
    this.init()
}

ActorQuery.prototype.init = function () {
    this.html = {
        view: document.getElementById("actor-search-screen"),
        input: document.getElementById("actor-search-input"),
        result: document.getElementById("actor-search-result"),
        close: document.getElementById("actor-search-close"),
    }
    this.html.input.addEventListener("input", this.keyboardClick.bind(this))
    this.html.close.addEventListener("click", this.close.bind(this))
    this.html.view.addEventListener("click", this.close.bind(this))
    this.html.view.firstElementChild.addEventListener("click", function (event) {
        event.stopPropagation()
    })
}

ActorQuery.prototype.reset = function () {
    this.html.input.value = ""
    $(this.html.result).empty()
}

ActorQuery.prototype.open = function () {
    this.reset()
    this.html.view.style.display = "flex"
    this.html.input.focus()
}

ActorQuery.prototype.close = function (id) {
    this.html.view.style.display = "none"
    this.callback(this.actors.find(a => a.id == id))
}

ActorQuery.prototype.keyboardClick = function () {
    this.sendRequest();
}

ActorQuery.prototype.sendRequest = function () {
    $.ajax({
        type: "GET",
        url: "/Search/Actors",
        data: {
            query: this.html.input.value
        },
        complete: function (data) {
            this.unpackResults(data.responseText)
            this.populate();
        },
        context: this,
        traditional: true
    })
}

ActorQuery.prototype.unpackResults = function (data) {
    this.actors = []
    let nodes = $($.parseHTML(data)).filter(".body-content")
        .children().toArray().filter(m => m.classList.contains("actor-card"))
    for (let node of nodes) {
        let wrapper = document.createElement("div")
        wrapper.appendChild(node);
        let id = node.dataset.id;
        if (this.selected.find(x => x.id == id))
            continue;
        wrapper.addEventListener("click", function (event) {
            event.preventDefault()
            this.close(id)
        }.bind(this))
        this.actors.push({
            id: id,
            html: wrapper,
            name: node.firstElementChild.children[1].firstElementChild.textContent,
            img: node.firstElementChild.firstElementChild.style.backgroundImage
        })
    }
}

ActorQuery.prototype.populate = function () {
    let ref = this.html.result
    while (ref.firstElementChild)
        ref.removeChild(ref.firstElementChild)

    for (let actor of this.actors) {
        ref.appendChild(actor.html)
    }
}