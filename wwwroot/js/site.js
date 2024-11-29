// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('DOMContentLoaded', function () {
    const editButtons = document.querySelectorAll('.edit-review');

    editButtons.forEach((button) => {
        button.addEventListener('click', function () {
            const reviewId = this.dataset.reviewId;
            const comment = this.dataset.reviewComment;
            const rating = this.dataset.reviewRating;

            document.getElementById('editReviewId').value = reviewId;
            document.getElementById('editReviewComment').value = comment;
            document.querySelector(`#editReviewModal input[value="${rating}"]`).checked = true;

            new bootstrap.Modal(document.getElementById('editReviewModal')).show();
        });
    });
});
