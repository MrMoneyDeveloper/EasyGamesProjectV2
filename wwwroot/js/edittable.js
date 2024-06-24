$(document).ready(function () {
    // Open modal to edit comment
    $(document).on('click', '.edit-comment', function () {
        var transactionId = $(this).data('transaction-id');
        var comment = $(this).data('comment');
        $('#editTransactionId').val(transactionId);
        $('#editComment').val(comment);
        $('#editCommentModal').modal('show');
    });

    // Save edited comment
    $('#saveCommentButton').click(function () {
        var transactionId = $('#editTransactionId').val();
        var comment = $('#editComment').val();

        $.ajax({
            url: '/api/transaction/update-comments',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify([{ TransactionID: transactionId, Comment: comment }]),
            success: function () {
                alert('Comment updated successfully');
                $('#editCommentModal').modal('hide');
                location.reload(); // Reload the page to see the changes
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('AJAX Error:', textStatus, errorThrown);
                var errorMessage = $('#errorMessage');
                var errorDetails = `Error: ${jqXHR.responseJSON?.message}\n\nStatus: ${jqXHR.status}\n\nResponse: ${JSON.stringify(jqXHR.responseJSON)}`;
                errorMessage.text(errorDetails);
                errorMessage.show();
            }
        });
    });
});