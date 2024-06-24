function validateClientForm() {
    var balance = $('#balance').val();
    if (balance < 0) {
        alert('Initial Investment cannot be negative');
        return false;
    }
    return true;
}

function validateTransactionForm() {
    var amount = $('#transactionAmount').val();
    if (amount < 0) {
        alert('Transaction Amount cannot be negative');
        return false;
    }
    return true;
}

$('#addClientForm').submit(function (event) {
    event.preventDefault();
    if (validateClientForm()) {
        var client = {
            name: $('#name').val(),
            surname: $('#surname').val(),
            clientBalance: $('#balance').val()
        };
        addClient(client);
    }
});

$('#addTransactionForm').submit(function (event) {
    event.preventDefault();
    if (validateTransactionForm()) {
        var transaction = {
            clientID: $('#transactionClientId').val(),
            amount: $('#transactionAmount').val(),
            transactionTypeID: $('#transactionType').val(),
            comment: $('#transactionComment').val()
        };
        addTransaction(transaction);
    }
});
