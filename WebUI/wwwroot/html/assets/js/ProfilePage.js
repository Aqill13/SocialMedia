
// Toast Notification
function toast(icon, message) {
    const Toast = Swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        customClass: {
            title: 'custom-toast-title'
        },
        didOpen: (toast) => {
            if (icon === 'success')
                toast.style.border = "1px solid #28a745";
            else if (icon === 'error')
                toast.style.border = "1px solid #dc3545";
            else
                toast.style.border = "1px solid warning";
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    Toast.fire({
        icon: icon,
        title: message
    });
}

// AJAX Tab Loading
$(document).on("click", ".profile-tab-ajax", function (e) {
    e.preventDefault();

    const url = $(this).attr("href");

    $("#profileMainContent").load(url, function () {
        $(".profile-tab-ajax").removeClass("active");
        $(`a[href='${url}']`).addClass("active");
    });
});

// AJAX About Section Loading
$(document).on("click", ".about-ajax", function (e) {
    e.preventDefault();

    const url = $(this).attr("href");

    $("#profileTabContent").load(url, function () {
        $(".about-ajax").removeClass("active");
        $(`a[href='${url}']`).addClass("active");
    });
});

// Section View/Edit Toggle
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


// Education
function resetEducationForm() {
    $("#educationId").val("");
    $("#educationSchool").val("");
    $("#educationField").val("");
    $("#educationDegree").val("");
    $("#educationStart").val("");
    $("#educationEnd").val("");
    $("#educationFormTitle").text("Add Education");
    $("#educationSubmitBtn").text("Add");
}
function openEducationForm() {
    $("#workEduTab").removeClass("show active");
    $("#educationFormSection").addClass("show active");
}
$(document).on("click", "#educationAddBtn", function () {
    resetEducationForm();
    openEducationForm();
});
$(document).on("click", ".education-edit-btn", function (e) {
    e.preventDefault();

    const id = $(this).data("id");
    const school = $(this).data("school");
    const field = $(this).data("field");
    const degree = $(this).data("degree");
    const start = $(this).data("start");
    const end = $(this).data("end");

    $("#educationId").val(id);
    $("#educationSchool").val(school);
    $("#educationField").val(field);
    $("#educationDegree").val(degree);
    $("#educationStart").val(start);
    $("#educationEnd").val(end);

    $("#educationFormTitle").text("Edit Education");
    $("#educationSubmitBtn").text("Save");

    openEducationForm();
});

// Work Experience
function resetWorkForm() {
    $("#workId").val("");
    $("#workCompany").val("");
    $("#workPosition").val("");
    $("#workStart").val("");
    $("#workEnd").val("");
    $("#workFormTitle").text("Add Work");
    $("#workSubmitBtn").text("Add");
}
function openWorkForm() {
    $("#workEduTab").removeClass("show active");
    $("#workFormSection").addClass("show active");
}
$(document).on("click", "#workAddBtn", function () {
    resetWorkForm();
    openWorkForm();
});
$(document).on("click", ".work-edit-btn", function (e) {
    e.preventDefault();

    const id = $(this).data("id");
    const company = $(this).data("company");
    const position = $(this).data("position");
    const start = $(this).data("start");
    const end = $(this).data("end");

    $("#workId").val(id);
    $("#workCompany").val(company);
    $("#workPosition").val(position);
    $("#workStart").val(start);
    $("#workEnd").val(end);

    $("#workFormTitle").text("Edit Work");
    $("#workSubmitBtn").text("Save");

    openWorkForm();
});

// Update Profile Info Form
$(document).on("submit", "#profileInfoForm", function (e) {
    e.preventDefault();

    const form = $(this);

    $.ajax({
        url: "/User/Profile/UpdateProfileInfo",
        type: "POST",
        data: form.serialize(),
        success: function (res) {
            if (!res || !res.success) {
                toast("error", res.message || "Update failed");
                return;
            }
            const d = res.data;
            if (d.bio !== undefined) {
                $("#profileInfoBioText").text(d.bio || "No information entered").toggleClass("text-warning", !d.bio);
            }
            if (d.location !== undefined) {
                $("#profileInfoLocationText").text(d.location || "No information entered").toggleClass("text-warning", !d.location);
            }
            if (d.birthDate !== undefined) {
                $("#profileInfoBirthDateText").text(d.birthDate || "No information entered").toggleClass("text-warning", !d.birthDate);
            }
            if (d.birthplace !== undefined) {
                $("#profileInfoBirthplaceText").text(d.birthplace || "No information entered").toggleClass("text-warning", !d.birthplace);
            }
            if (d.livesIn !== undefined) {
                $("#profileInfoLivesInText").text(d.livesIn || "No information entered").toggleClass("text-warning", !d.livesIn);
            }
            if (d.gender !== undefined) {
                $("#profileInfoGenderText").text(d.gender || "No information entered").toggleClass("text-warning", !d.gender);
            }
            if (d.status !== undefined) {
                $("#profileInfoStatusText").text(d.status || "No information entered").toggleClass("text-warning", !d.status);
            }

            $("#profileInfoEditTab").removeClass("show active");
            $("#profileInfoViewTab").addClass("show active");
            toast("success", res.message);
        },
        error: function () {
            toast("error", "Could not update profile information");
        }
    });
});

// update hobbies And Interests
$(document).on("submit", "#hobbiesAndInterestsForm", function (e) {
    e.preventDefault();

    const form = $(this);

    $.ajax({
        url: "/User/Profile/UpdateHobbiesAndInterests",
        type: "POST",
        data: form.serialize(),
        success: function (res) {
            if (!res || !res.success) {
                toast("error", res.message || "Update failed");
                return;
            }
            const d = res.data;
            if (d.hobbies !== undefined) {
                $("#hobbiesHobbiesText").text(d.hobbies || "No information entered").toggleClass("text-warning", !d.hobbies);
            }
            if (d.favoriteMovies !== undefined) {
                $("#hobbiesFavMoviesText").text(d.favoriteMovies || "No information entered").toggleClass("text-warning", !d.favoriteMovies);
            }
            if (d.favoriteGames !== undefined) {
                $("#hobbiesFavGamesText").text(d.favoriteGames || "No information entered").toggleClass("text-warning", !d.favoriteGames);
            }
            if (d.favoriteBooks !== undefined) {
                $("#hobbiesFavBooksText").text(d.favoriteBooks || "No information entered").toggleClass("text-warning", !d.favoriteBooks);
            }

            $("#hobbiesEditTab").removeClass("show active");
            $("#hobbiesViewTab").addClass("show active");
            toast("success", res.message);
        },
        error: function () {
            toast("error", "Could not update hobbies and interests");
        }
    });
});

// work add/edit form submit
$(document).on("submit", "#workForm", function (e) {
    e.preventDefault();

    const form = $(this);

    $.ajax({
        url: "/User/Profile/AddOrUpdateWorkExperience",
        type: "POST",
        data: form.serialize(),
        success: function (res) {
            if (!res || !res.success) {
                toast("error", res.message || "Work experience update failed");
                return;
            }
            const d = res.data;
            $("#workNoInfoText").remove();
            const itemId = `#workItem_${d.id}`;
            const existingItem = $(itemId);
            const dateText = `${d.startDateDisplay} - ${d.endDateDisplay}`;
            if (existingItem.length) {
                existingItem.find(".work-company-text").text(d.companyName);
                existingItem.find(".work-position-text").text(d.position);
                existingItem.find(".work-date-text").text(dateText);

                const editLink = existingItem.find(".work-edit-btn");
                editLink.data("company", d.companyName);
                editLink.data("position", d.position);
                editLink.data("start", d.startDateValue);
                editLink.data("end", d.endDateValue);
            } else {
                const newItemHtml = `
                <li class="d-flex mb-4 align-items-center justify-content-between work-item" id="workItem_${d.id}" data-work-id="${d.id}">
                    <div class="w-100">
                        <div class="d-flex justify-content-between align-items-center">
                            <div class="d-flex align-items-center gap-1">
                                <i class="fa-solid fa-circle-check text-success"></i>
                                <div class="ms-3">
                                    <h6 class="work-company-text">${d.companyName}</h6>
                                    <p class="mb-0 work-position-text">${d.position}</p>
                                    <p class="mb-0 work-date-text">${dateText}</p>
                                </div>
                            </div>
                            <div class="edit-relation">
                                <a href="#"
                                   class="d-flex align-items-center gap-1 work-edit-btn"
                                   data-id="${d.id}"
                                   data-company="${d.companyName}"
                                   data-position="${d.position}"
                                   data-start="${d.startDateValue}"
                                   data-end="${d.endDateValue}">
                                    <i class="ph ph-pencil-simple"></i>
                                    <span class="edit-btn">Edit</span>
                                </a>
                                <a href="#"
                                   class="d-flex align-items-center gap-1 text-danger work-delete-btn mt-2" data-id="${d.id}">
                                    <i class="ph ph-trash"></i>
                                    <span class="delete-btn">Delete</span>
                                </a>
                            </div>
                        </div>
                    </div>
                </li>`;

                $("#workList").append(newItemHtml);
            }
            $("#workFormSection").removeClass("show active");
            $("#workEduTab").addClass("show active");
            toast("success", res.message);
        },
        error: function (xhr) {
            toast("error", "Could not update work experience");
        }
    });
});
// Delete Work Experience
$(document).on("click", ".work-delete-btn", function (e) {
    e.preventDefault();
    const id = $(this).data("id");
    const itemId = `#workItem_${id}`;
    Swal.fire({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to restore this experience!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/User/Profile/DeleteWorkExperience",
                type: "POST",
                data: { id: id },
                success: function (res) {
                    if (!res || !res.success) {
                        toast("error", res.message || "Work experience deletion failed")
                        return;
                    }
                    $(itemId).remove();
                    if ($("#workList").children().length === 0) {
                        $("#workList").html('<p id="workNoInfoText" class="text-warning">No information entered</p>');
                    }
                    toast("success", res.message);
                },
                error: function () {
                    toast("error", "Could not delete work experience");
                }
            });
        }
    });
});


// education add/edit form submit
$(document).on("submit", "#educationForm", function (e) {
    e.preventDefault();

    const form = $(this);

    $.ajax({
        url: "/User/Profile/AddOrUpdateEducation",
        type: "POST",
        data: form.serialize(),
        success: function (res) {
            if (!res || !res.success) {
                toast("error", res.message || "Education update failed")
                return;
            }
            const d = res.data;
            $("#educationNoInfoText").remove();
            const itemId = `#eduItem_${d.id}`;
            const existingItem = $(itemId);
            const dateText = `${d.startDateDisplay} - ${d.endDateDisplay}`;
            if (existingItem.length) {
                existingItem.find(".edu-school-text").text(d.schoolName);
                existingItem.find(".edu-fielddegree-text").text(`${d.degree} - ${d.field}`);
                existingItem.find(".edu-date-text").text(dateText);

                const editLink = existingItem.find(".education-edit-btn");
                editLink.data("school", d.companyName);
                editLink.data("field", d.field);
                editLink.data("degree", d.degree);
                editLink.data("start", d.startDateValue);
                editLink.data("end", d.endDateValue);
            } else {
                const newItemHtml = `
                <li id="eduItem_${d.id}" data-edu-id="${d.id}" class="d-flex mb-4 align-items-center justify-content-between">
                        <div class="w-100">
                            <div class="d-flex justify-content-between align-items-center">

                                <div class="d-flex align-items-center gap-1">
                                    <i class="fa-solid fa-circle-check text-success"></i>
                                    <div class="ms-3">
                                        <h6 class="edu-school-text">${d.schoolName}</h6>
                                        <p class="edu-fielddegree-text mb-0">${d.degree} - ${d.field}</p>
                                        <p class="edu-date-text mb-0">${dateText}</p>
                                    </div>
                                </div>
                                    <div class="edit-relation">
                                        <a href="#"
                                           class="d-flex align-items-center gap-1 education-edit-btn"
                                           data-id="${d.id}"
                                           data-school="${d.schoolName}"
                                           data-field="${d.field}"
                                           data-degree="${d.degree}"
                                           data-start="${d.startDateValue}"
                                           data-end="${d.endDateValue}">
                                            <i class="ph ph-pencil-simple"></i>
                                            <span class="edit-btn">Edit</span>
                                        </a>
                                        <a href="#"
                                           class="d-flex align-items-center gap-1 text-danger edu-delete-btn mt-2" data-id="${d.id}">
                                            <i class="ph ph-trash"></i>
                                            <span class="delete-btn">Delete</span>
                                        </a>
                                    </div>
                            </div>
                        </div>
                    </li>`;

                $("#educationList").append(newItemHtml);
            }
            $("#educationFormSection").removeClass("show active");
            $("#workEduTab").addClass("show active");
            toast("success", res.message);
        },
        error: function (xhr) {
            toast("error", "Could not update work experience");
        }
    });
});
// Delete education
$(document).on("click", ".edu-delete-btn", function (e) {
    e.preventDefault();
    const id = $(this).data("id");
    const itemId = `#eduItem_${id}`;
    Swal.fire({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to restore this experience!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/User/Profile/DeleteEducation",
                type: "POST",
                data: { id: id },
                success: function (res) {
                    if (!res || !res.success) {
                        toast("error", res.message || "Education deletion failed");
                        return;
                    }
                    $(itemId).remove();
                    if ($("#educationList").children().length === 0) {
                        $("#educationList").html('<p id="educationNoInfoText" class="text-warning">No information entered</p>');
                    }
                    toast("success", res.message);
                },
                error: function () {
                    toast("error", "Could not delete work experience");
                }
            });
        }
    });
});


// User account Private / Public
$(document).on("change", "#privateAccCheckInp", function () {
    const isPrivate = $(this).is(":checked");
    $.ajax({
        url: "/User/Profile/TogglePrivateAccount",
        type: "post",
        data: { isPrivate: isPrivate },
        success: function (res) {
            if (!res || !res.success) {
                toast("error", res.message || "Could not update privacy setting");
                return;
            }
            if (isPrivate) {
                $("#privateAccStatusText").text("Your account is private").addClass("text-primary");
            } else {
                $("#privateAccStatusText").text("Your account is public").removeClass("text-primary");
            }
            toast("success", res.message);
        },
        error: function () {
            toast("error", "Connection error");
        }
    });
});


// Update Profile Visibility
$(document).on("change", "#visibilitySelect", function () {
    const field = $(this).data("field");
    const visibility = $(this).val();

    $.ajax({
        url: "/User/Profile/UpdateVisibility",
        type: "POST",
        data: { field: field, visibility: visibility },
        success: function () {
            toast("success", `${field} field changed to ${visibility}`);
        },
        error: function () {
            toast("error", "Could not update visibility");
        }
    });
});





// Add social link Modal
$(document).on("submit", "#addSocialLinkForm", function (e) {
    e.preventDefault();
    const platform = $("#platform").val();
    const socialUrl = $("#socialUrl").val();
    $.ajax({
        url: "/User/Profile/AddSocialLink",
        type: "post",
        data: { platform: platform, url: socialUrl },
        success: function (response) {

            if (response.success) {
                $("#addSocialLinkModal").modal('hide');
                $("#platform").val("");
                $("#socialUrl").val("");
                $("#noSocialLinksPlaceholder").remove();
                $("#platform option[value='" + response.platformValue + "']").remove();
                var remaining = $("#platform option").length;
                if (remaining === 0) {
                    $("#AddasocialaccountLink").hide();
                }
                $("#social-data-block").append(`
                        <li class="text-center">
                            <a href="${response.url}">
                                <i class="${response.iconClass}"></i>
                            </a>
                        </li>
                    `);
                toast("success", response.message);
            }
        },
        error: function (err) {
            toast("error", "Error");
        }
    })
})

// Profile Photo Update

let cropper;
function openCropperModal(event) {
    const file = event.target.files[0];
    if (!file) return;

    const img = document.getElementById("profileImageCropTarget");
    img.src = URL.createObjectURL(file);

    const modalEl = document.getElementById('profileImageCropModal');
    const modal = new bootstrap.Modal(modalEl);
    modal.show();

    modalEl.addEventListener('shown.bs.modal', function handler() {
        if (cropper) {
            cropper.destroy();
        }

        cropper = new Cropper(img, {
            aspectRatio: 1,        
            viewMode: 1,
            autoCropArea: 1,
            background: false,
            movable: true,
            zoomable: true,
            scalable: false,
            rotatable: false
        });
        modalEl.removeEventListener('shown.bs.modal', handler);
    });
    modalEl.addEventListener('hidden.bs.modal', function () {
        if (cropper) {
            cropper.destroy();
            cropper = null;
        }
        document.getElementById("imageUploadInp").value = "";
    });
}
function uploadCroppedProfileImage() {
    if (!cropper) return;

    const canvas = cropper.getCroppedCanvas({
        width: 300,
        height: 300
    });

    canvas.toBlob(function (blob) {
        let formData = new FormData();
        formData.append('file', blob, 'avatar.png');

        $.ajax({
            url: '/User/Profile/ChangeProfileImage',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res && res.success) {
                    const newSrc = res.imageUrl + '?v=' + new Date().getTime();
                    $("#headerProfileImg").attr("src", newSrc);
                    $("#profileImageCropModal").modal("hide");
                    toast("success", res.message);
                    $("#profileImageMenuText").text("Change photo");
                    $("#profileImageRemoveItem").removeClass("d-none");
                }
                else {
                    toast("error", res.message || "Profile image update failed");
                }
            }
        });
    }, 'image/png',0.9);
}

// Remove Profile Image
$(document).on("click", "#profileImageRemoveItem", function (e) {
    e.preventDefault();
    $.ajax({
        url: '/User/Profile/RemoveProfileImage',
        type: 'POST',
        success: function (res) {
            if (res && res.success) {
                const newSrc = res.imageUrl + '?v=' + new Date().getTime();
                $("#headerProfileImg").attr("src", newSrc);
                toast("success", res.message);
                $("#profileImageMenuText").text("Add photo");
                $("#profileImageRemoveItem").addClass("d-none");
            }
            else {
                toast("error", res.message || "Profile image removal failed");
            }
        }
    });
});

// Cover Photo Update
let coverCropper;
function openCoverImgCropperModal(event) {
    const file = event.target.files[0];
    if (!file) return;

    const img = document.getElementById("coverImageCropTarget");
    img.src = URL.createObjectURL(file);

    const modalEl = document.getElementById('coverImageCropModal');
    const modal = new bootstrap.Modal(modalEl);
    modal.show();

    modalEl.addEventListener('shown.bs.modal', function handler() {
        if (coverCropper) {
            coverCropper.destroy();
        }

        coverCropper = new Cropper(img, {
            aspectRatio: 4/1,        
            viewMode: 1,
            autoCropArea: 1,
            background: false,
            movable: true,
            zoomable: true,
            scalable: false,
            rotatable: false
        });
        modalEl.removeEventListener('shown.bs.modal', handler);
    });
    modalEl.addEventListener('hidden.bs.modal', function () {
        if (coverCropper) {
            coverCropper.destroy();
            coverCropper = null;
        }
        document.getElementById("coverImageUploadInp").value = "";
    });
}
function uploadCroppedCoverImage() {
    if (!coverCropper) return;

    const canvas = coverCropper.getCroppedCanvas({
        width: 1200,
        height: 300
    });

    canvas.toBlob(function (blob) {
        let formData = new FormData();
        formData.append('file', blob, 'avatar.png');

        $.ajax({
            url: '/User/Profile/ChangeCoverImage',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res && res.success) {
                    const newSrc = res.imageUrl + '?v=' + new Date().getTime();
                    $("#headerCoverImg").attr("src", newSrc);
                    $("#coverImageCropModal").modal("hide");
                    toast("success", res.message);
                    $("#coverImageMenuText").text("Change photo");
                    $("#coverImageRemoveItem").removeClass("d-none");
                }
            }
        });
    }, 'image/png');
}

// Remove Cover Image
$(document).on("click", "#coverImageRemoveItem", function (e) {
    e.preventDefault();
    $.ajax({
        url: '/User/Profile/RemoveCoverImage',
        type: 'POST',
        success: function (res) {
            if (res && res.success) {
                const newSrc = res.imageUrl + '?v=' + new Date().getTime();
                $("#headerCoverImg").attr("src", newSrc);
                toast("success", res.message);
                $("#coverImageMenuText").text("Add photo");
                $("#coverImageRemoveItem").addClass("d-none");
            }
            else {
                toast("error", res.message || "Cover image removal failed");
            }
        }
    });
});
