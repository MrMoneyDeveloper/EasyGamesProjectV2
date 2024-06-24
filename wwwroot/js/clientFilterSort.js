let filterCurrentPage = 1; // Initialize current page
let filterTotalPages = 1; // Initialize total pages
let filter = '';
let sort = '';

function handleFilterError(jqXHR) {
    var errorMessage = $('#filterErrorMessage');
    var errorDetails = `Error: ${jqXHR.responseJSON?.message}\n\nStatus: ${jqXHR.status}\n\nResponse: ${JSON.stringify(jqXHR.responseJSON)}`;
    errorMessage.text(errorDetails);
    errorMessage.show();
}

function loadFilteredClients(page) {
    // Ensure default values for filter and sort
    const effectiveFilter = filter || '_';
    const effectiveSort = sort || '_';

    console.log(`Loading clients for page: ${page} with filter: ${effectiveFilter} and sort: ${effectiveSort}`);
    $.ajax({
        url: `/api/client/get-all?page=${page}&pageSize=10&filter=${encodeURIComponent(effectiveFilter)}&sort=${encodeURIComponent(effectiveSort)}`,
        method: 'GET',
        success: function (data, textStatus, jqXHR) {
            console.log('Data received:', data);
            if (data && data.length > 0) {
                var clientTableBody = $('#clientTableBody');
                clientTableBody.empty();
                data.forEach(function (client) {
                    clientTableBody.append(
                        `<tr>
                            <td>${client.clientID}</td>
                            <td>${client.name}</td>
                            <td>${client.surname}</td>
                            <td>${client.clientBalance}</td>
                            <td>
                                <button class="btn btn-info btn-sm view-transactions" data-client-id="${client.clientID}">Modify Transactions</button>
                                <button class="btn btn-success btn-sm add-transaction" data-client-id="${client.clientID}">Add Transaction</button>
                            </td>
                        </tr>`
                    );
                });
                $('#filterErrorMessage').hide();

                // Calculate total pages from response headers if available
                const totalItems = jqXHR.getResponseHeader('X-Total-Count'); // Adjust header name as per your API
                if (totalItems) {
                    filterTotalPages = Math.ceil(totalItems / 10); // Assuming pageSize is 10
                }

                // Update pagination
                updatePagination();
            } else {
                handleFilterError({ responseJSON: { message: 'No clients found' } });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status === 404) {
                console.log('No clients found, looping back to the first page.');
                filterCurrentPage = 1;
                loadFilteredClients(filterCurrentPage);
            } else {
                console.error('AJAX Error:', textStatus, errorThrown);
                handleFilterError(jqXHR);
            }
        }
    });
}

// Function to update pagination controls
function updatePagination() {
    $('#prevPage').toggleClass('disabled', filterCurrentPage === 1);
    $('#nextPage').toggleClass('disabled', filterCurrentPage === filterTotalPages);
}

// Event listeners for filter and sort inputs
$('#filterInput').on('input', function () {
    filter = $(this).val();
    filterCurrentPage = 1;
    loadFilteredClients(filterCurrentPage);
});

$('#sortSelect').change(function () {
    sort = $(this).val();
    filterCurrentPage = 1;
    loadFilteredClients(filterCurrentPage);
});

// Event listeners for pagination controls
$('#prevPage').click(function (e) {
    e.preventDefault();
    if (filterCurrentPage > 1) {
        filterCurrentPage--;
        loadFilteredClients(filterCurrentPage);
    }
});

$('#nextPage').click(function (e) {
    e.preventDefault();
    if (filterCurrentPage < filterTotalPages) {
        filterCurrentPage++;
        loadFilteredClients(filterCurrentPage);
    }
});

// Load initial data
$(document).ready(function () {
    loadFilteredClients(filterCurrentPage); // Automatically load clients on page load

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
