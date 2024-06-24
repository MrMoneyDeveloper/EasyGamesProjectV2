$(document).ready(function () {
    $(document).on('click', '.view-transactions', function () {
        var clientId = $(this).data('client-id');
        console.log('Modify button clicked, clientId:', clientId);
        if (clientId) {
            window.location.href = `/transaction/edit/${clientId}`;
        } else {
            alert('Client ID is required to view transactions.');
        }
    });

    $(document).on('click', '.add-transaction', function () {
        var clientId = $(this).data('client-id');
        console.log('Add Transaction button clicked, clientId:', clientId);
        if (clientId) {
            window.location.href = `/transaction/add?clientId=${clientId}`;
        } else {
            alert('Client ID is required to add a transaction.');
        }
    });
});


