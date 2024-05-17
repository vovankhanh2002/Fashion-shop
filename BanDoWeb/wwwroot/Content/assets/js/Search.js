$(document).ready(function () {
    $('#newsletter').click(function () {
        var txtNewsletter = $('#txtNewsletter').val()
        $.ajax({
            url: "/admin/newsletter/create",
            data: { newsletter: txtNewsletter },
            method: 'POST',
            success: function (res) {

            }
        })
    })

    var suggestionsProduct = document.getElementById('search');
    suggestionsProduct.style.display = 'none';
    var suggestionsPro = document.getElementById('products');

    document.getElementById('searchInput').addEventListener('input', function () {
        var searchTerm = $('#searchInput').val();
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                if (xhr.status === 200) {
                    var response = JSON.parse(xhr.responseText);
                    var suggestionsList = document.getElementById('liSearch');
                    
                    suggestionsList.innerHTML = ''; // Xóa gợi ý cũ trước khi thêm gợi ý mới
                    if (response != null) {
                        response.forEach(function (suggestion) {
                            const listItem = document.createElement('li');
                            listItem.style.listStyle = 'none';

                            // Tạo phần tử div với lớp row
                            const rowDiv = document.createElement('div');
                            rowDiv.classList.add('row');
                            rowDiv.style.marginTop ="15px"
                            // Tạo phần tử div với lớp col-md-6
                            const col1Div = document.createElement('div');
                            col1Div.classList.add('col-md-9');
                            // Tạo phần tử a và thiết lập href
                            const aElement = document.createElement('a');
                            aElement.href = '/detail/index/' + suggestion.id;

                            // Tạo phần tử img và thiết lập src
                            const image = document.createElement('img');
                            image.src = `Content/assets/img/Product/${suggestion.imageUrl}` // Đường dẫn hình ảnh
                            image.style.width = "60px"
                            aElement.appendChild(image);

                            // Tạo phần tử span và thiết lập nội dung là 'TiTil'
                            const titleSpan = document.createElement('span');
                            titleSpan.classList = "font-weight-normal"
                            titleSpan.style.color = "black"
                            titleSpan.textContent = suggestion.title;
                            aElement.appendChild(titleSpan);

                            // Gắn phần tử a vào phần tử div col-md-6
                            col1Div.appendChild(aElement);

                            // Tạo phần tử div với lớp col-md-6
                            const col2Div = document.createElement('div');
                            col2Div.classList.add('col-md-3');

                            // Tạo phần tử p và thiết lập nội dung là 'Gia'
                            const pricePara = document.createElement('p');
                            pricePara.textContent = new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(suggestion.price);
                            pricePara.style.color = "red"
                            titleSpan.style.textAlign = "center"
                            col2Div.appendChild(pricePara);

                            rowDiv.appendChild(col1Div);
                            rowDiv.appendChild(col2Div);
                            listItem.appendChild(rowDiv);
                            suggestionsList.appendChild(listItem);
                            suggestionsProduct.style.display = 'block';
                            suggestionsPro.style.display = 'block'
                        });
                    } else {
                        suggestionsProduct.style.display = 'none';
                        suggestionsPro.style.display = 'none'
                    }
                } else {
                    console.error('Lỗi khi gửi yêu cầu:', xhr.status);
                }
            }
        };

        xhr.open('GET', '/shop/GetProductSuggestions?searchTerm=' + searchTerm, true);
        xhr.send();
    });


})

