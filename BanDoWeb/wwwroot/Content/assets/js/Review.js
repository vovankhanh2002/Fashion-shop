$(document).ready(function () {
    var rating = null
    $('.star').on('click', function () {
        rating = $(this).data('rating');
        $('.star').removeClass('active');
        $(this).prevAll('.star').addBack().addClass('active');
    });

    $('#btnReview').on('click', function () {
        var idProduct = $('#idProduct').val()
        var name = $('#name').val()
        var email = $('#email').val()
        var messageReview = $('#messageReview').val()
        var objReviews = {
            name: name,
            email: email,
            messageReview: messageReview,
            productId: idProduct,
            star: rating
        }
        if (rating != null) {
            $.ajax({
                url: '/detail/ReviewProduct',
                method: 'POST',
                data: { reviews: objReviews },
                success: function (response) {
                    if (response.success) {
                        location.reload()
                    } else {
                        location.href = "https://localhost:7043/Identity/Account/Login?returnUrl=" + window.location.pathname
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error:', error);
                }
            })
        } else {
            alert("Bạn chưa chọn sao đánh giá")
        }
    })
})