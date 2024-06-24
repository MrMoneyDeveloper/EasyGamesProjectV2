$(document).ready(function () {
    var currentPage = 1;

    loadClients(currentPage); // Automatically load clients on page load

    $('#prevPage').click(function (e) {
        e.preventDefault();
        if (currentPage > 1) {
            currentPage--;
            loadClients(currentPage);
        }
    });

    $('#nextPage').click(function (e) {
        e.preventDefault();
        currentPage++;
        loadClients(currentPage);
    });
});
