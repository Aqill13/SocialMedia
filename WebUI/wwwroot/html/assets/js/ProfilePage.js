$(document).on("click", ".profile-tab-ajax", function (e) {
    e.preventDefault();

    const url = $(this).attr("href");

    $("#profileMainContent").load(url, function () {
        $(".profile-tab-ajax").removeClass("active");
        $(`a[href='${url}']`).addClass("active");
    });
});

$(document).on("click", ".about-ajax", function (e) {
    e.preventDefault();

    const url = $(this).attr("href");

    $("#profileTabContent").load(url, function () {
        $(".about-ajax").removeClass("active");
        $(`a[href='${url}']`).addClass("active");
    });
});

function showSectionView(viewId, editId) {
    $(editId).removeClass("active show");
    $(viewId).addClass("active show");
}

function showSectionEdit(viewId, editId) {
    $(viewId).removeClass("active show");
    $(editId).addClass("active show");
}

$(document).on("click", ".section-edit-btn", function () {
    const viewId = $(this).data("view");
    const editId = $(this).data("edit");
    showSectionEdit(viewId, editId);
});

$(document).on("click", ".section-cancel-btn", function () {
    const viewId = $(this).data("view");
    const editId = $(this).data("edit");
    showSectionView(viewId, editId);
});
