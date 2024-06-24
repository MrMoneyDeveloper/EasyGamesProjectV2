$(document).ready(function () {
    // Show success modal and reload page on clicking OK button
    $('#successModal').on('show.bs.modal', function () {
        $('#reloadPageButton').click(function () {
            location.reload();
        });
    });
});

function handleError(jqXHR) {
    var errorMessage = $('#errorMessage');
    var errorDetails = `Error: ${jqXHR.responseJSON?.message}\n\nStatus: ${jqXHR.status}\n\nResponse: ${JSON.stringify(jqXHR.responseJSON)}`;
    errorMessage.text(errorDetails);
    errorMessage.show();
}

function addTransaction(transaction) {
    $.ajax({
        url: '/api/transaction/add',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(transaction),
        success: function (response) {
            $('#addTransactionForm')[0].reset();
            $('#errorMessage').hide();
            $('#successModal').modal('show'); // Show success modal
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('AJAX Error:', textStatus, errorThrown);
            handleError(jqXHR);
        }
    });
}

function loadTransactions(clientId) {
    if (!clientId) {
        console.error('Client ID is undefined');
        return;
    }
    console.log(`Loading transactions for client ID: ${clientId}`);
    $.ajax({
        url: `/api/transaction/get-by-client/${clientId}`,
        method: 'GET',
        success: function (data) {
            var transactionTableBody = $('#transactionTableBody');
            transactionTableBody.empty();

            let netAmount = 0;

            data.forEach(function (transaction, index) {
                let amountDisplay = transaction.amount;
                if (transaction.transactionTypeName === 'Credit') {
                    amountDisplay = -transaction.amount;
                }

                netAmount += parseFloat(amountDisplay); // Correctly accumulate the net amount

                transactionTableBody.append(
                    `<tr>
                        <td>${index + 1}</td>
                        <td style="color:${transaction.transactionTypeName === 'Credit' ? 'red' : 'green'}">${amountDisplay.toFixed(2)}</td>
                        <td>${transaction.transactionTypeName}</td>
                        <td>${transaction.comment}</td>
                        <td>${transaction.transactionDate}</td>
                    </tr>`
                );
            });

            $('#currentTotal').text(`Transaction Total: ${netAmount.toFixed(2)}`);
            $('#clientBalance').text(`Client Balance: ${netAmount.toFixed(2)}`);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('AJAX Error:', textStatus, errorThrown);
            handleError(jqXHR);
        }
    });
}

$('#addTransactionForm').submit(function (event) {
    event.preventDefault();
    var amount = parseFloat($('#transactionAmount').val());
    var transactionType = $('#transactionType').val();

    if (transactionType == "2" && amount > 0) { // Assuming "2" represents Credit
        amount = -amount;
    }

    var transaction = {
        clientID: $('#transactionClientId').val(),
        amount: amount,
        transactionTypeID: transactionType,
        comment: '' // Comment will be set by the server
    };

    addTransaction(transaction);
});

$(document).ready(function () {
    $('#viewPreviousTransactions').click(function () {
        $('#previousTransactionsSection').show();
        $('#addNewTransactionSection').hide();
        loadTransactions($('#transactionClientId').val()); // Load transactions on initial load
    });

    $('#addNewTransactionTab').click(function () {
        $('#previousTransactionsSection').hide();
        $('#addNewTransactionSection').show();
    });

    // Load transactions initially
    loadTransactions($('#transactionClientId').val());
});

$(document).ready(function () {
    $('#saveCommentsButton').click(function () {
        var commentUpdates = [];
        $('.comment-field').each(function () {
            var transactionId = $(this).data('transaction-id');
            var comment = $(this).val();
            commentUpdates.push({ TransactionID: transactionId, Comment: comment });
        });

        $.ajax({
            url: '/api/transaction/update-comments',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(commentUpdates),
            success: function () {
                alert('Comments updated successfully');
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
