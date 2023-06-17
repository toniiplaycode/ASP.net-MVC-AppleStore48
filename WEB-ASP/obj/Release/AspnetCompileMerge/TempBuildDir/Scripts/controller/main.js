// Chức năng đóng mở search header
let btnIcon = document.querySelector('.js-modal-search-icon');
let modal = document.querySelector('.js-modal-search');
let modalContainer = document.querySelector('.js-modal-container');


function showSearch(event) {
    event.preventDefault();
    modal.classList.add('open');
}
function hideSearch() {
    modal.classList.remove('open');
}

btnIcon.addEventListener('click', showSearch);

modal.addEventListener('click', hideSearch);

modalContainer.addEventListener('click', function (event) {
    event.stopPropagation();
})

// Next and Prev Slider
window.addEventListener('load', function () {
    const slider = document.querySelector('.slider');
    const sliderMain = document.querySelector('.slider-main');
    const nextBtn = document.querySelector('.slider-next');
    const sliderItems = document.querySelectorAll('.banner__item');
    const prevBtn = document.querySelector('.slider-prev');
    const dorItems = document.querySelectorAll('.slider-dot-item');
    const sliderItemsWidth = sliderItems[0].offsetWidth;

    let index = 0;
    let position = 0;

    let slideInterval = setInterval(function () {
        handleChangeSlide(1);
    }, 5000);

    [...dorItems].forEach(item =>
        item.addEventListener('click', function (count) {
            [...dorItems].forEach(el => el.classList.remove('active'))
            count.target.classList.add('active')
            const slideIndex = parseInt(count.target.dataset.index);
            index = slideIndex;
            position = -1 * index * sliderItemsWidth
            sliderMain.style = `transform: translateX(${position}px)`;
        }));

    nextBtn.addEventListener('click', function () {
        handleChangeSlide(1);
    });

    prevBtn.addEventListener('click', function () {
        handleChangeSlide(-1);

    });

    function handleChangeSlide(direction) {
        if (direction == 1) {
            if (index >= sliderItems.length - 1) {
                index = sliderItems.length - 1;
                position = -1 * index * sliderItemsWidth;
                sliderMain.style = `transform: translateX(${position}px)`;
                setTimeout(function () {
                    index = 0;
                    position = 0;
                    sliderMain.style = `transform: translateX(${position}px)`;
                    [...dorItems].forEach(el => el.classList.remove('active'));
                    dorItems[index].classList.add('active');
                }, 3000);
                return;
            };
            position = position - sliderItemsWidth;
            sliderMain.style = `transform: translateX(${position}px)`;
            index++;
        }

        else if (direction == -1) {
            if (index <= 0) {
                index = 0;
                return;
            };
            position = position + sliderItemsWidth;
            sliderMain.style = `transform: translateX(${position}px)`;
            index--;
        }
        [...dorItems].forEach(el => el.classList.remove('active'));
        dorItems[index].classList.add('active');
    }
});


// Click show and hide

let moreShows = document.querySelector('.read-more-link');

moreShows.addEventListener('click',function(){
    moreShows.parentNode.classList.toggle('active');
})


window.addEventListener("scroll", function () {
    var container = document.querySelector(".cart-price-container");
    var distanceFromBottom = document.body.clientHeight - (container.offsetTop + container.offsetHeight);

    if (distanceFromBottom <= 0) {
        container.style.position = "absolute";
        container.style.bottom = 0;
    } else {
        container.style.position = "sticky";
        container.style.bottom = null;
    }
});
var slideIndex = 1;
showDivs(slideIndex);

function plusDivs(n) {
    showDivs(slideIndex += n);
}

function currentDiv(n) {
    showDivs(slideIndex = n);
}

function showDivs(n) {
    var i;
    var x = document.getElementsByClassName("mySlides");
    var slideMini = document.getElementsByClassName("demo");
    if (n > x.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = x.length }
    for (i = 0; i < x.length; i++) {
        x[i].style.display = "none";
    }
    x[slideIndex - 1].style.display = "block";

    for (i = 0; i < slideMini.length; i++) {
        slideMini[i].classList.replace('show', 'hide')
    }
    slideMini[slideIndex - 1].classList.add('show');
}

// Click tăng giảm input
function decreaseValue() {
    var quantityInput = document.getElementById('quantityInput');
    var currentValue = parseInt(quantityInput.value);
    if (currentValue > 1) {
        quantityInput.value = currentValue - 1;
    }
    var id = currentValue + 1;
    $.post("/Cart/Update_Quantity_Cart/" + id, (ev) => {
    })
}

function increaseValue() {
    var quantityInput = document.getElementById('quantityInput');
    var currentValue = parseInt(quantityInput.value);
    quantityInput.value = currentValue + 1;
    var id = currentValue + 1;
    $.post("/Cart/Update_Quantity_Cart/" + id, (ev) => {
    })
}


