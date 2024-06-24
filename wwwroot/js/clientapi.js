let totalPages = 1; // Initialize total pages

function handleError(jqXHR) {
    var errorMessage = $('#errorMessage');
    var errorDetails = `Error: ${jqXHR.responseJSON?.message}\n\nStatus: ${jqXHR.status}\n\nResponse: ${JSON.stringify(jqXHR.responseJSON)}`;
    errorMessage.text(errorDetails);
    errorMessage.show();
}

function loadClients(page) {
    console.log(`Loading clients for page: ${page}`);
    $.ajax({
        url: `/api/client/get-all?page=${page}&pageSize=10`,
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
                $('#errorMessage').hide();

                // Calculate total pages from response headers if available
                const totalItems = jqXHR.getResponseHeader('X-Total-Count'); // Adjust header name as per your API
                if (totalItems) {
                    totalPages = Math.ceil(totalItems / 10); // Assuming pageSize is 10
                }
            } else {
                handleError({ responseJSON: { message: 'No clients found' } });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status === 404) {
                console.log('No clients found, looping back to the first page.');
                currentPage = 1;
                loadClients(currentPage);
            } else {
                console.error('AJAX Error:', textStatus, errorThrown);
                handleError(jqXHR);
            }
        }
    });
}

$(document).ready(function () {
    var currentPage = 1;

    // Load clients initially
    loadClients(currentPage);

    $('#prevPage').click(function (e) {
        e.preventDefault();
        if (currentPage > 1) {
            currentPage--;
            loadClients(currentPage);
        } else {
            currentPage = totalPages;
            loadClients(currentPage);
        }
    });

    $('#nextPage').click(function (e) {
        e.preventDefault();
        currentPage++;
        if (currentPage > totalPages) {
            currentPage = 1; // Loop back to the first page
        }
        loadClients(currentPage);
    });
});
