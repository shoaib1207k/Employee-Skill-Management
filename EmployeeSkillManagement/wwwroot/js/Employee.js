jQuery(document).ready(function ($) {
    // adding skill in create employee form
    $("#addSkill").on("click", function (event) {
        event.preventDefault();
        // var selectedOptionText = ;
        let level = $("#levelSelect").val();
        let skillId = $("#SelectedSkillIds").val();
        let yoe = $("#skillYoe").val();
        let isPrimary = $("#isPrimarySkill").prop("checked");
        let skill = $("#SelectedSkillIds")
            .find("option[value='" + skillId + "']")
            .text();
        let skillIndex = $(".skill-level").length;
        // Check if the skill is already added
        let isSkillAdded = false;
        $("#skillsContainer .skill-level").each(function () {
            let existingSkillId = $(this).find(".skill-id-input").val();
            if (existingSkillId == skillId) {
                isSkillAdded = true;
                return false; // Break the loop
            }
        });
        if (isSkillAdded) {
            $("#skill-form-error").show();
            $("#skill-form-error").text("This skill is already added!");
        } else if (
            skill != null &&
            level != null &&
            skillId != null &&
            yoe != null &&
            level != 0 &&
            skillId != 0 &&
            yoe >= 0
        ) {
            let primaryStar = '';
            if(isPrimary){
                primaryStar='<i class="bi bi-star-fill text-warning me-1" title="Primary skill"></i>';
            }
            console.log(skillId);
            let skill_level =
                `<div class="skill-level d-flex justify-content-around align-items-center border border-primary rounded col-3">` +
                primaryStar + skill + ` Level-` + level +` | YOE- ` + yoe +`
                                    <div class="btn remove-skill"> 
                                        <i class="bi bi-x-circle btn-outline-danger rounded-circle"></i>
                                    </div>
                                    <input class="skill-id-input" type="text" value="${skillId}" name="EmployeeSkillsAndLevels[${skillIndex}].SkillId" hidden>
                                    <input type="text" value="${skill}" name="EmployeeSkillsAndLevels[${skillIndex}].SkillName" hidden>
                                    <input type="text" value="${level}" name="EmployeeSkillsAndLevels[${skillIndex}].SkillLevel" hidden>
                                    <input type="text" value="${yoe}" name="EmployeeSkillsAndLevels[${skillIndex}].SkillExperience" hidden>
                                    <input type="text" value="${isPrimary}" name="EmployeeSkillsAndLevels[${skillIndex}].IsPrimary" hidden>

                                </div>`;
            $("#skillsContainer").append(skill_level);
            $("#SelectedSkillIds").prop("selectedIndex", 0);
            $("#levelSelect").prop("selectedIndex", 0);
            $("#skillYoe").val("0");
            $("#skillYoe").prop("placeholder", "Experience");
            $("#isPrimarySkill").prop("checked", false);
            $("#skill-form-error").hide();
        } else {
            $("#skill-form-error").show();
            $("#skill-form-error").text(
                "Please choose a skill with valid proficiency level and experience"
            );
            $("#SelectedSkillIds").addClass("input-validation-error");
            $("#levelSelect").addClass("input-validation-error");
            $("#skillYoe").addClass("input-validation-error");
        }
    });


    $("#DateOfJoining").change(function(){
        console.log($(this).val());
        $(this).val($(this).val());
    })


    $('[data-bs-toggle="tooltip"]').tooltip();

    $(document).on("click", ".remove-skill", function () {
        $(this).closest(".skill-level").remove();
        updateSkillIndex();
    });

    $(".save-btn").on("click", function (e) {
        if (!$(".skill-level").length > 0 && $("form").valid()) {
            e.preventDefault();
            $("#skill-form-error").show();
            $("#skill-form-error").text(
                "Please choose a skill with proficiency level and experience"
            );
            $("#SelectedSkillIds").addClass("input-validation-error");
            $("#levelSelect").addClass("input-validation-error");
            if ($("#skillYoe").val() == 0) {
                console.log(34);
                $("#skillYoe").addClass("input-validation-error");
            }
        }

        if (!$(".skill-level").length > 0 && !$("form").valid()) {
            $("#skill-form-error").show();
            $("#skill-form-error").text(
                "Please choose a skill and proficiency level"
            );
            $("#SelectedSkillIds").addClass("input-validation-error");
            $("#levelSelect").addClass("input-validation-error");
            // $("#skillYoe").addClass("input-validation-error");
            if ($("#skillYoe").val() == 0) {
                $("#skillYoe").removeClass("valid");
                $("#skillYoe").addClass("input-validation-error");
            }
        }
    });

    // Delete ajax function
    $(document).on("click", ".delete-employee", function () {
        let employee_id = $(this).attr("data-emp-id");
        try {
            $.ajax({
                url: "/Employee/GetEmployeeDeleteModal",
                type: "POST",
                data: { employee_id: employee_id },
                success: function (response) {
                    $("#deleteModal").html(response);
                    $("#deleteModal .modal").modal("show");
                },
            });
        } catch (error) {
            console.log(error);
        }
    });

    let timeOut;
    $("#filterNameOrId").on("input", function () {
        let value = $(this).val();
        if (value && value.length > 3) {
            clearTimeout(timeOut);
            timeOut = setTimeout(() => {
                employeesFilterAjax();
            }, 500);
        }
        if (!value) {
            employeesFilterAjax();
        }
    });

    $("#filterSkill").on("change", function () {
        employeesFilterAjax();
    });

    let reportTable = $("#generatedReport").DataTable({
        dom: "Bfrtip",
        buttons: ["excel", "pdf"],
        searching: false,
        pageLength: 15,
    });

    if ($("#generatedReport").length > 0) {
        // Check the total number of records and apply styling
        if (reportTable.page.info().recordsDisplay <= 15) {
            $(".dataTables_paginate, .dataTables_info").hide();
        }

        // Listen for changes to the page length
        reportTable.on("length.dt", function (e, settings, len) {
            // Apply styling when the total number of records is less than 20
            if (table.page.info().recordsDisplay <= 15) {
                $(".dataTables_paginate, .dataTables_info").hide();
            } else {
                $(".dataTables_paginate, .dataTables_info").show();
            }
        });
    }

    // update indexing on remove
    function updateSkillIndex() {
        $(".skill-level").each(function (index) {
            $(this)
                .find('input[name$=".SkillId"]')
                .attr("name", `EmployeeSkillsAndLevels[${index}].SkillId`);
            $(this)
                .find('input[name$=".SkillName"]')
                .attr("name", `EmployeeSkillsAndLevels[${index}].SkillName`);
            $(this)
                .find('input[name$=".SkillLevel"]')
                .attr("name", `EmployeeSkillsAndLevels[${index}].SkillLevel`);
            $(this)
                .find('input[name$=".SkillExperience"]')
                .attr(
                    "name",
                    `EmployeeSkillsAndLevels[${index}].SkillExperience`
                );
            $(this)
                .find('input[name$=".IsPrimary"]')
                .attr("name", `EmployeeSkillsAndLevels[${index}].IsPrimary`);
        });
    }

    function employeesFilterAjax() {
        let empNameOrId = $("#filterNameOrId").val();
        let skillId = $("#filterSkill").val();
        $("#spinner-container").removeClass("d-none");
        let filterData = {
            empNameOrId: empNameOrId,
            skillId: skillId,
        };

        try {
            $.ajax({
                url: "/Employee/SearchEmployee",
                type: "POST",
                data: filterData,
                success: function (response) {
                    $("#spinner-container").addClass("d-none");
                    $("#employeeListDiv").html(response);
                },
            });
        } catch (error) {
            console.log(error);
        }
    }
});
