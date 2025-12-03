
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

// AJAX Friends Section Loading
$(document).on("click", ".friendsTabsItem", function (e) {
    e.preventDefault();

    const url = $(this).attr("href");

    $("#friendsTabContent").load(url, function () {
        $(".friendsTabsItem").removeClass("active");
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
        url: "/User/ProfileSettings/UpdateProfileInfo",
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
        url: "/User/ProfileSettings/UpdateHobbiesAndInterests",
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
        url: "/User/Work/AddOrUpdateWorkExperience",
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
                url: "/User/Work/DeleteWorkExperience",
                type: "POST",
                data: { id: id },
                success: function (res) {
                    if (!res || !res.success) {
                        toast("error", res.message || "Work experience deletion failed")
                        return;
                    }
                    const item = $(itemId);
                    item.remove();
                    const aboutList = $("#workList");
                    if (aboutList.length && aboutList.find("li[data-work-id]").length === 0) {
                        if ($("#workNoInfoText").length === 0) {
                            aboutList.append('<li class="text-warning" id="workNoInfoText">No information entered</li>');
                        }
                    }
                    const editList = $("#editProfileWorkListUl");
                    if (editList.length && editList.children("li").length === 0) {
                        editList.html('<li class="text-muted" id="workNoInfoText">No work experience added yet</li>');
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
        url: "/User/Education/AddOrUpdateEducation",
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
                editLink.data("school", d.schoolName);
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
                url: "/User/Education/DeleteEducation",
                type: "POST",
                data: { id: id },
                success: function (res) {
                    if (!res || !res.success) {
                        toast("error", res.message || "Education deletion failed");
                        return;
                    }
                    const item = $(itemId);
                    item.remove();
                    const aboutList = $("#educationList");
                    if (aboutList.length && aboutList.find("li[data-edu-id]").length === 0) {
                        if ($("#educationNoInfoText").length === 0) {
                            aboutList.append('<li class="text-warning" id="educationNoInfoText">No information entered</li>');
                        }
                    }
                    const editList = $("#editProfileEduListUl");
                    if (editList.length && editList.children("li").length === 0) {
                        editList.html('<li class="text-muted" id="educationNoInfoText">No education added yet</li>');
                    }
                    toast("success", res.message);
                },
                error: function () {
                    toast("error", "Could not delete education");
                }
            });
        }
    });
});


// User account Private / Public
$(document).on("change", "#privateAccCheckInp", function () {
    const isPrivate = $(this).is(":checked");
    $.ajax({
        url: "/User/ProfileSettings/TogglePrivateAccount",
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
        url: "/User/ProfileSettings/UpdateVisibility",
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
    if (!socialUrl || socialUrl.trim() === "") {
        toast("error", "Please enter a valid URL");
        return;
    }
    if (!platform || platform.trim() === "") {
        toast("error", "Please select a platform");
        return;
    }
    $.ajax({
        url: "/User/SocialLinks/AddSocialLink",
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
                if (remaining === 1) {
                    $("#AddasocialaccountLink").hide();
                }
                $("#social-data-block").append(`
                        <li class="text-center">
                            <a href="${response.url}">
                                <i id="socialPlatformIconHE" class="${response.iconClass}"></i>
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

        //document.getElementById("imageUploadInp").value = "";
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
            url: '/User/Media/ChangeProfileImage',
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
    }, 'image/png', 0.9);
}

// Remove Profile Image
$(document).on("click", "#profileImageRemoveItem", function (e) {
    e.preventDefault();
    $.ajax({
        url: '/User/Media/RemoveProfileImage',
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
            aspectRatio: 4 / 1,
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
            url: '/User/Media/ChangeCoverImage',
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
        url: '/User/Media/RemoveCoverImage',
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




// ------------------ EDIT PROFILE PAGE --------------------

// PERSONAL INFO update
$(document).on("submit", "#editProfileBasicInfoForm", function (e) {
    e.preventDefault();
    const formData = new FormData(this);
    $.ajax({
        url: "/User/ProfileSettings/EditProfile",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        headers: { "X-Requested-With": "XMLHttpRequest" },
        success: function (res) {
            if (!res || !res.success) {
                toast("error", res.message || "Update failed");
                return;
            }
            const d = res.data;
            console.log(d);
            $("#editProfileLastnameInp").val(d.lastName || "");
            $("#editProfileFirstnameInp").val(d.firstName || "");
            $("#editProfileBioInp").val(d.bio || "");
            $("#editProfileLocationInp").val(d.location || "");
            $("#editProfileBirthDateInp").val(d.birthDate || "");
            $("#editProfileBirthplaceInp").val(d.birthplace || "");
            $("#editProfileLivesInInp").val(d.livesIn || "");
            $("#editProfileGender").val(d.gender || "");
            $("#editProfileStatus").val(d.status || "");
            if (d.imageUrl) {
                const newProfileImg = d.imageUrl + "?v=" + new Date().getTime();
                $("#editProfileProfileImg").attr("src", newProfileImg);
                $("#headerProfileImg").attr("src", newProfileImg);
            }
            if (d.coverImageUrl) {
                const newCoverImg = d.coverImageUrl + "?v=" + new Date().getTime();
                $("#editProfileCoverImg").attr("src", newCoverImg);
                $("#headerCoverImg").attr("src", newCoverImg);
            }

            toast("success", res.message);
        },
    });
});


// WORK add/edit form submit
$(document).on("submit", "#workFormEditProfilePage", function (e) {
    e.preventDefault();

    const form = $(this);

    $.ajax({
        url: "/User/Work/AddOrUpdateWorkExperience",
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
                <li id="workItem_${d.id}" data-work-id="${d.id}" class="border rounded-3 p-3 mb-2 d-flex justify-content-between align-items-center">
                    <div>
                        <h6 class="work-company-text">${d.companyName}</h6>
                        <p class="mb-0 text-muted work-position-text">${d.position}</p>
                        <p class="mb-0 text-muted work-date-text">
                            ${dateText}
                        </p>
                    </div>

                    <div class="edit-relation">
                        <a href="#" class="d-flex align-items-center gap-1 work-edit-btn"
                           data-id="${d.id}"
                           data-company="${d.companyName}"
                           data-position="${d.position}"
                           data-start="${d.startDateValue}"
                           data-end="${d.endDateValue}" data-bs-toggle="modal"
                           data-bs-target="#editProfileAddWorkModal">
                            <i class="ph ph-pencil-simple"></i>
                            <span class="edit-btn">Edit</span>
                        </a>
                        <a href="#" data-id="${d.id}" class="d-flex align-items-center gap-1 text-danger work-delete-btn mt-2">
                            <i class="ph ph-trash"></i>
                            <span class="delete-btn">Delete</span>
                        </a>
                    </div>
                </li>`;

                $("#editProfileWorkListUl").append(newItemHtml);
            }
            const modalEl = document.getElementById("editProfileAddWorkModal");
            const modalInstance = bootstrap.Modal.getInstance(modalEl);
            modalInstance.hide();
            resetWorkForm();
            toast("success", res.message);
        },
        error: function (xhr) {
            toast("error", "Could not update work experience");
        }
    });
});

// EDUCATION add/edit form submit
$(document).on("submit", "#eduFormEditProfilePage", function (e) {
    e.preventDefault();

    const form = $(this);

    $.ajax({
        url: "/User/Education/AddOrUpdateEducation",
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
                editLink.data("school", d.schoolName);
                editLink.data("field", d.field);
                editLink.data("degree", d.degree);
                editLink.data("start", d.startDateValue);
                editLink.data("end", d.endDateValue);
            } else {
                const newItemHtml = `
                <li id="eduItem_${d.id}" data-edu-id="${d.id}" class="border rounded-3 p-3 mb-2 d-flex justify-content-between align-items-center">
                    <div>
                        <h6 class="edu-school-text">${d.schoolName}</h6>
                        <p class="mb-0 text-muted edu-fielddegree-text">${d.degree} - ${d.field}</p>

                        <p class="mb-0 text-muted edu-date-text">
                            ${dateText}
                        </p>
                    </div>

                    <div class="edit-relation">
                        <a href="#" class="d-flex align-items-center gap-1 education-edit-btn"
                           data-id="${d.id}"
                           data-school="${d.schoolName}"
                           data-field="${d.field}"
                           data-degree="${d.degree}"
                           data-start="${d.startDateValue}"
                           data-end="${d.endDateValue}" data-bs-toggle="modal"
                           data-bs-target="#editProfileAddEduModal">
                            <i class="ph ph-pencil-simple"></i>
                            <span class="edit-btn">Edit</span>
                        </a>
                        <a href="#" data-id="${d.id}" class="d-flex align-items-center gap-1 text-danger edu-delete-btn mt-2">
                            <i class="ph ph-trash"></i>
                            <span class="delete-btn">Delete</span>
                        </a>
                    </div>
                </li>`;

                $("#editProfileEduListUl").append(newItemHtml);
            }
            const modalEl = document.getElementById("editProfileAddEduModal");
            const modalInstance = bootstrap.Modal.getInstance(modalEl);
            modalInstance.hide();
            resetEducationForm();
            toast("success", res.message);
        },
        error: function (xhr) {
            toast("error", "Could not update work experience");
        }
    });
});

// HOBBIES And INTERESTS update
$(document).on("submit", "#editProfileHobbiesForm", function (e) {
    e.preventDefault();

    const form = $(this);

    $.ajax({
        url: "/User/ProfileSettings/UpdateHobbiesAndInterests",
        type: "POST",
        data: form.serialize(),
        success: function (res) {
            if (!res || !res.success) {
                toast("error", res.message || "Update failed");
                return;
            }
            const d = res.data;
            $("#editProfileHobbiesInp").val(d.hobbies || "");
            $("#editProfileFavMoviesInp").val(d.favoriteMovies || "");
            $("#editProfileFavGamesInp").val(d.favoriteGames || "");
            $("#editProfileFavBooksInp").val(d.favoriteBooks || "");
            toast("success", res.message);
        },
        error: function () {
            toast("error", "Could not update hobbies and interests");
        }
    });
});

// Profile Image
function uploadCroppedProfileImageEditProfile() {
    if (!cropper) return;

    const canvas = cropper.getCroppedCanvas({
        width: 300,
        height: 300
    });

    canvas.toBlob(function (blob) {
        if (!blob) return;
        const croppedFile = new File([blob], "profile-image.png", { type: "image/png" });

        const dataTransfer = new DataTransfer();
        dataTransfer.items.add(croppedFile);

        const input = document.getElementById("imageUploadInp");
        input.files = dataTransfer.files;
        document.getElementById("deleteProfileImageFlag").value = "false";
        const previewImg = document.querySelector("#editProfileProfileImg");
        if (previewImg) {
            previewImg.src = URL.createObjectURL(croppedFile);
            $("#editProfileImageMenuText").text("Change photo");
            $("#editProfileImageRemoveItem").removeClass("d-none");
        }

        $("#profileImageCropModal").modal("hide");
    }, "image/png");
}
function editProfileDeleteProfileImage() {
    const input = document.getElementById("imageUploadInp");
    input.value = "";
    document.getElementById("deleteProfileImageFlag").value = "true";
    const previewImg = document.querySelector("#editProfileProfileImg");
    if (previewImg) {
        previewImg.src = "/uploads/profilep/default-profile-account.jpg";
        $("#editProfileImageMenuText").text("Add photo");
        $("#editProfileImageRemoveItem").addClass("d-none");
    }
}

// Cover Image
function uploadCroppedCoverImageEditProfile() {
    if (!coverCropper) return;

    const canvas = coverCropper.getCroppedCanvas({
        width: 1200,
        height: 300
    });

    canvas.toBlob(function (blob) {
        if (!blob) return;
        const croppedFile = new File([blob], "profile-image.png", { type: "image/png" });

        const dataTransfer = new DataTransfer();
        dataTransfer.items.add(croppedFile);

        const input = document.getElementById("coverImageUploadInp");
        input.files = dataTransfer.files;
        document.getElementById("deleteCoverImageFlag").value = "false";
        const previewImg = document.querySelector("#editProfileCoverImg");
        if (previewImg) {
            previewImg.src = URL.createObjectURL(croppedFile);
            $("#editProfileCoverImageMenuText").text("Change photo");
            $("#editProfileCoverImageRemoveItem").removeClass("d-none");
        }

        $("#coverImageCropModal").modal("hide");
    }, "image/png");
}
function editProfileDeleteCoverImage() {
    const input = document.getElementById("coverImageUploadInp");
    input.value = "";
    document.getElementById("deleteCoverImageFlag").value = "true";
    const previewImg = document.querySelector("#editProfileCoverImg");
    if (previewImg) {
        previewImg.src = "/uploads/cover/default-cover-img.png";
        $("#editProfileCoverImageMenuText").text("Add photo");
        $("#editProfileCoverImageRemoveItem").addClass("d-none");
    }
}

// Social Links Visibility
$(document).on("click", ".social-visible-btn", function (e) {
    e.preventDefault();
    const el = $(this);
    const id = el.data("id");
    const isVisible = el.data("visible") === true || el.data("visible") === "true";
    let newVisible = !isVisible;
    $.ajax({
        url: "/User/SocialLinks/UpdateSocialLinkVisibility",
        type: "POST",
        data: { id: id, isVisible: newVisible },
        success: function (res) {
            if (!res || !res.success) {
                toast("error", res.message || "Could not update social link visibility");
                return;
            }
            else {
                if (newVisible) {
                    el.removeClass("text-info").addClass("text-warning");
                    el.find("i").removeClass("fa-eye-slash").addClass("fa-eye");
                    el.find(".visible-btn").text("Visible");
                } else {
                    el.removeClass("text-warning").addClass("text-info");
                    el.find("i").removeClass("fa-eye").addClass("fa-eye-slash");
                    el.find(".visible-btn").text("Hidden");
                }
                el.data("visible", newVisible);
                toast("success", res.message);
            }
        },
        error: function () {
            toast("error", "Could not update social link visibility");
        }
    });
});

// Social Links Edit
$(document).on("click", ".social-edit-btn", function (e) {
    e.preventDefault();
    const el = $(this);
    const id = el.data("id");
    const platform = el.data("platform");
    const url = el.data("url");
    let iconClass = "";
    switch (platform.toLowerCase()) {
        case "facebook":
            iconClass = "fa-brands fa-facebook-f fs-1";
            break;
        case "twitter":
            iconClass = "fa-brands fa-x-twitter fs-1";
            break;
        case "instagram":
            iconClass = "fa-brands fa-instagram fs-1";
            break;
        case "linkedin":
            iconClass = "fa-brands fa-linkedin-in fs-1";
            break;
        case "github":
            iconClass = "fa-brands fa-github fs-1";
            break;
        case "youtube":
            iconClass = "fa-brands fa-youtube fs-1";
            break;
        default:
            iconClass = "fas fa-globe fs-1";
    }
    $("#editProfileSocialLinkModalIcon").attr("class", iconClass);
    $("#editSocialIdModal").val(id);
    $("#editSocialUrlModal").val(url);
    $("#editProfileSocialLinkModal").modal("show");
});

$(document).on("click", "#modalSaveSocialEditBtn", function (e) {
    const id = $("#editSocialIdModal").val();
    const url = $("#editSocialUrlModal").val();
    if (!url || url.trim() === "") {
        toast("error", "Please enter a valid URL");
        return;
    }
    $.ajax({
        url: "/User/SocialLinks/UpdateSocialLink",
        type: "POST",
        data: { id: id, url: url },
        success: function (res) {
            if (!res || !res.success) {
                toast("error", res.message || "Could not update social link");
                return;
            }
            else {
                const item = $(`#socialItem-${id}`);
                item.find(".socialUrlP")
                    .text(url.length > 35 ? url.substring(0, 35) + "..." : url);
                item.find(".social-edit-btn").data("url", url);

                $("#editProfileSocialLinkModal").modal("hide");
                toast("success", res.message);
            }
        },
        error: function () {
            toast("error", "Could not update social link");
        }
    });
});

// Delete Social Link
$(document).on("click", ".social-delete-btn", function (e) {
    e.preventDefault();
    const el = $(this);
    const id = el.data("id");
    Swal.fire({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to restore this social link!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/User/SocialLinks/DeleteSocialLink",
                type: "POST",
                data: { id: id },
                success: function (res) {
                    if (!res || !res.success) {
                        toast("error", res.message || "Could not delete social link");
                        return;
                    }
                    const item = $(`#socialItem-${id}`);
                    item.remove();
                    $("#newSocialPlatform").append(new Option(res.platformName, res.platformValue));
                    $("#addSocialLinkBoxEditProfile").removeClass("d-none");
                    toast("success", res.message);
                },
                error: function () {
                    toast("error", "Could not delete social link");
                }
            });
        }
    });
});

// Add New Social link
$(document).on("click", "#editProfileAddSocialBtn", function (e) {
    const platform = $("#newSocialPlatform").val();
    const url = $("#newSocialUrl").val();

    if (!url || url.trim() === "") {
        toast("error", "Please enter a valid URL");
        return;
    }
    if (!platform || platform.trim() === "") {
        toast("error", "Please select a platform");
        return;
    }

    $.ajax({
        url: "/User/SocialLinks/AddSocialLink",
        type: "POST",
        data: { platform: platform, url: url },
        success: function (res) {
            if (!res || !res.success) {
                toast("error", res.message || "Could not add social link");
                return;
            }
            const visibleHtml = `
                <a role="button" data-id="${res.id}" data-visible="true"
                   class="text-warning d-flex align-items-center gap-1 mt-1 social-visible-btn">
                    <i class="fa-solid fa-eye"></i>
                    <span class="visible-btn">Visible</span>
                </a>
            `;
            $("#editProfileSocialListUl").append(`
                <li class="col-md-6" id="socialItem-${res.id}">
                    <div class="d-flex align-items-center justify-content-between border rounded-3 p-2 px-3">
                        <div class="d-flex align-items-center gap-3">
                            <i id="socialPlatformIconHE" class="${res.iconClass}"></i>
                            <div class="d-flex flex-column">
                                <label class="form-label mb-0 fs-6">${res.platform}</label>
                                <p class="mb-0 socialUrlP">${res.url}</p>
                            </div>
                        </div>
                        <div class="edit-relation">
                            <a role="button" class="d-flex align-items-center gap-1 social-edit-btn"
                               data-id="${res.id}"
                               data-url="${res.url}"
                               data-platform="${res.platform}"
                               data-bs-target="#editProfileSocialLinkModal" data-bs-toggle="modal">
                                <i class="ph ph-pencil-simple"></i>
                                <span class="edit-btn">Edit</span>
                            </a>
                            ${visibleHtml}
                            <a role="button" data-id="${res.id}"
                               class="d-flex align-items-center gap-1 text-danger social-delete-btn mt-1">
                                <i class="ph ph-trash"></i>
                                <span class="delete-btn">Delete</span>
                            </a>
                        </div>
                    </div>
                </li>
            `);
            $("#newSocialPlatform option[value='" + res.platformValue + "']").remove();
            if ($("#newSocialPlatform option").length === 1) {
                $("#addSocialLinkBoxEditProfile").addClass("d-none");
            }
            $("#newSocialUrl").val("");
            toast("success", res.message);
        },
        error: function () {
            toast("error", "Could not add social link");
        }
    });
});

