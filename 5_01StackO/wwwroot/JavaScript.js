$(() => {
    alert('foo');
    const Id = $('.imageId').val();

    setInterval(() => {
        $.post("/home/getLikes", { Id: Id }, function (likesCount) {
            $('.likeAmount').val(likesCount);
            console.log('set')
        });
    }, 1000)

    $('.likeBtn').on('click', function () {
        const imageId = $(this).data('id');
        $.post("/home/addLike", { Id: imageId }, function (message) {
            alert(message)

        })
    })


});