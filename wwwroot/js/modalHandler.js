$(document).ready(function () {
    // Show success modal and reload page on clicking OK button
    $('#successModal').on('show.bs.modal', function () {
        $('#reloadPageButton').click(function () {
            location.reload();
        });
    });
});
