// Chức năng đóng mở search header
let btnIcon = document.querySelectorAll('.js-modal-search-icon');
let modal = document.querySelectorAll('.js-modal-search');
let modalContainer = document.querySelectorAll('.js-modal-container');

for (let i = 0; i < modal.length; i++) {
    btnIcon[i].addEventListener('click', (event) => {
        event.preventDefault();
        modal[i].classList.add('open');
    });

    modal[i].addEventListener('click', () => {
        modal[i].classList.remove('open');
    });

    modalContainer[i].addEventListener('click', function (event) {
        event.stopPropagation();
    })
}

// Next and Prev Slider
window.addEventListener('load', function() {
    const slider = document.querySelector('.slider');
    const sliderMain = document.querySelector('.slider-main');
    const nextBtn = document.querySelector('.slider-next');
    const sliderItems = document.querySelectorAll('.banner__item');
    const prevBtn = document.querySelector('.slider-prev');
    const dorItems = document.querySelectorAll('.slider-dot-item');
    const sliderItemsWidth = sliderItems[0].offsetWidth;

    let index = 0;
    let position = 0;

    let slideInterval = setInterval(function() {
        handleChangeSlide(1);
      }, 5000);

    [... dorItems].forEach(item => 
        item.addEventListener('click',function (count) {
            [... dorItems].forEach(el => el.classList.remove('active')) 
            count.target.classList.add('active')
            const slideIndex = parseInt(count.target.dataset.index);
            index = slideIndex;
            position = -1 * index * sliderItemsWidth
            sliderMain.style = `transform: translateX(${position}px)`;
    }));
   
    nextBtn.addEventListener('click', function() {
        handleChangeSlide(1);
    });

    prevBtn.addEventListener('click', function() {
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


window.addEventListener("scroll", function() {
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
    if (n > x.length) {slideIndex = 1}
    if (n < 1) {slideIndex = x.length}
    for (i = 0; i < x.length; i++) {
    x[i].style.display = "none";  
    }
    x[slideIndex-1].style.display = "block"; 

    for (i = 0; i < slideMini.length; i++) {
    slideMini[i].classList.replace('show', 'hide')
    }
    slideMini[slideIndex-1].classList.add('show');
}


function decreaseQuantity(IdProduct) {
    var quantityInput = document.getElementById("quantity_" + IdProduct);
    var quantity = parseInt(quantityInput.value);
    if (quantity > 1) {
        quantityInput.value = quantity - 1;
    }
}

function increaseQuantity(IdProduct) {
    var quantityInput = document.getElementById("quantity_" + IdProduct);
    var quantity = parseInt(quantityInput.value);
    quantityInput.value = quantity + 1;
}

//function updateQuantity(productId) {
//    var quantityInput = document.getElementById("quantity_" + productId);
//    var quantity = parseInt(quantityInput.value);

//    // Gửi yêu cầu cập nhật số lượng thông qua Ajax
//    // Sử dụng productId và quantity để gửi dữ liệu cần thiết đến action trong Controller
//    // Sau khi nhận được phản hồi từ action, bạn có thể làm gì đó để cập nhật giao diện nếu cần thiết
//}
