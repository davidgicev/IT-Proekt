function HorizontalML(list) {
    this.html = null
    this.width = null;
    this.height = null;
    this.nodes = []
    this.pages = null;
    this.rows = null;
    this.itemWidth = null;
    this.pageWidth = null;
    this.pageOffset = null;
    this.rowItemCount = null;
    this.window = {
        left: 1,
        right: 1
    }
    this.pageIndex = 0;
    this.init(list);
}

HorizontalML.prototype.configure = function (width, rows) {

    let itemDim = this.getItemSize()
    this.itemWidth = itemDim.width

    this.width = width;
    this.rows = Number(rows) || 1;

    this.rowItemCount = Math.floor(this.width / itemDim.width);

    if (this.rowItemCount * this.itemWidth / this.width > 0.9)
        this.rowItemCount--;

    this.pageWidth = this.itemWidth * this.rowItemCount;

    this.pageOffset = this.width * 0.5 - this.pageWidth * 0.5
}

HorizontalML.prototype.init = function (list) {

    while (list.childElementCount)
        this.nodes.push(list.removeChild(list.firstElementChild))

    this.configure(list.clientWidth, list.dataset.rows);
    this.groupItems();
    list.classList.remove("horizontal-ml-init")
    list.classList.add("horizontal-ml")
    let overlay = document.createElement("div")
    overlay.className = "horizontal-ml-overlay"
    let wrapper = document.createElement("div")
    wrapper.className = "horizontal-ml-wrapper"
    let arrowLeft = document.createElement("div")
    arrowLeft.innerHTML = "<"
    arrowLeft.onmousedown = this.slideLeft.bind(this)
    arrowLeft.className = "horizontal-ml-arrow-left"
    if (this.rows == 1)
        arrowLeft.style.marginBottom = "2em";
    let arrowRight = document.createElement("div")
    arrowRight.innerHTML = ">"
    arrowRight.className = "horizontal-ml-arrow-right"
    if (this.rows == 1)
        arrowRight.style.marginBottom = "2em";
    arrowRight.onmousedown = this.slideRight.bind(this)
    overlay.appendChild(arrowLeft)
    overlay.appendChild(arrowRight)
    let arr = document.createElement("div")
    wrapper.appendChild(arr)
    list.appendChild(overlay)
    list.appendChild(wrapper)
    this.html = {
        root: list,
        arrowLeft, arrowRight, overlay,
        window: arr
    }
    this.position();
    $(this.html.root).on("resizeElement", this.resizeElement.bind(this));
}

HorizontalML.prototype.resizeElement = function () {
    let index = this.pageIndex * this.rows * this.rowItemCount
    this.configure(this.html.root.clientWidth, this.rows)
    this.groupItems();
    index = Math.ceil(index / (this.rows * this.rowItemCount))
    this.pageIndex = Math.min(this.pages.length - 1, index)
    this.position();
    let koef = this.pageIndex == 0 ? 0 : 1;
    this.html.window.style.marginLeft = -koef * (this.pageWidth - this.pageOffset) + "px"
}

HorizontalML.prototype.position = function () {
    this.populate()
    this.updateArrows()
    for (let page of this.pages) {
        page.style.backgroundColor = "white"
    }
    this.pages[this.pageIndex].style.backgroundColor = "white"
}

HorizontalML.prototype.finishSlide = function () {
    if ($(this.html.window).is(":animated")) {
        $(this.html.window).finish();
    }
    event.preventDefault()
}

HorizontalML.prototype.slideRight = function (event) {
    this.finishSlide(event)
    this.pageIndex += 1;
    let koef = this.pageIndex == 1 ? 1 : 2;
    this.window.left++;
    this.position()
    $(this.html.window).animate(
        { marginLeft: -(koef * this.pageWidth - this.pageOffset) + "px" },
        "fast", this.popLeft.bind(this)
    )
}

HorizontalML.prototype.slideLeft = function (event) {
    this.finishSlide(event)
    this.pageIndex -= 1;

    if (this.pageIndex == 0) {
        this.window.right++;
        this.position()
        $(this.html.window).animate(
            { marginLeft: "0px" },
            "fast" , this.popRight.bind(this)
        )
    }
    else {
        this.window.right++;
        this.html.window.style.marginLeft =
            -((2) * this.pageWidth - this.pageOffset) + "px"
        this.position()
        $(this.html.window).animate(
            { marginLeft: -(this.pageWidth - this.pageOffset) + "px" },
            "fast", this.popRight.bind(this)
        )
    }
}

HorizontalML.prototype.popRight = function () {
    this.window.right--;
    this.populate();
}

HorizontalML.prototype.popLeft = function () {
    this.window.left--;
    this.html.window.style.marginLeft = -((this.window.left) * this.pageWidth - this.pageOffset) + "px"
    this.populate()
}

HorizontalML.prototype.populate = function () {
    let ref = this.html.window
    while (ref.firstElementChild) 
        ref.removeChild(ref.firstElementChild)

    let nodes = this.pages.slice(
        Math.max(this.pageIndex - this.window.left, 0),
        Math.min(this.pageIndex + this.window.right + 1, this.pages.length)
    )

    for (let node of nodes) {
        ref.appendChild(node)
    }
}

HorizontalML.prototype.updateArrows = function () {
    let left = false, right = false;
    if (this.pageIndex == 0) {
        this.html.arrowLeft.style.visibility = "hidden";
    }
    else {
        left = true;
        this.html.arrowLeft.style.visibility = "visible";
    }
    if (this.pageIndex < this.pages.length-1) {
        right = true;
        this.html.arrowRight.style.visibility = "visible";
    }
    else {
        this.html.arrowRight.style.visibility = "hidden";
    }

    let string = ""
    if (left && right)
        string = "white 0%, transparent 5% 95%, white 100%"
    else if (left)
        string = "white 0%, transparent 5%"
    else if (right)
        string = "transparent 95%, white 100%"

    this.html.overlay.style.background = "linear-gradient(to right, "+string+")"

}

HorizontalML.prototype.groupItems = function () {

    let items = []
    
    for (let node of this.nodes) {
        let itemWrapper = document.createElement("div")
        itemWrapper.appendChild(node)
        itemWrapper.style.width = this.itemWidth + "px";
        items.push(itemWrapper)
    }

    let rows = []
    let i = 0;
    let rowDisplayCount = this.rowItemCount;

    while (i < items.length) {
        let arr = items.slice(i, i += rowDisplayCount);
        let rowWrapper = document.createElement("div")
        for (let item of arr) {
            rowWrapper.appendChild(item)
        }
        rowWrapper.style.width = this.pageWidth + "px";
        //rowWrapper.style.height = this.height / this.rows + "px";
        rows.push(rowWrapper)
    }

    let pages = []
    i = 0;

    while (i < rows.length) {
        let arr = rows.slice(i, i += this.rows);
        let pageWrapper = document.createElement("div")
        for (let item of arr) {
            pageWrapper.appendChild(item)
        }
        pageWrapper.style.width = this.pageWidth + "px";
        //pageWrapper.style.height = this.height + "px";
        pages.push(pageWrapper)
    }

    this.pages = pages
}

HorizontalML.prototype.getItemSize = function () {
    let item = $(this.nodes[0]).clone().wrap("<div></div>").parent().get(0)
    item.style.position = "absolute"
    item.style.top = "-10000px"
    item.style.left = "-10000px"
    item.style.visibility = "hidden"
    document.body.appendChild(item)
    let dims = item.getBoundingClientRect()
    document.body.removeChild(item)
    return {
        width: Math.round(dims.width),
        height: Math.round(dims.height)
    }
}


window.addEventListener("load", function () {
    var lists = $(".horizontal-ml-init");
    for (let list of lists) {
        new HorizontalML(list);
    }
});

window.addEventListener("resize", function () {
    var lists = $(".horizontal-ml").trigger("resizeElement");
});