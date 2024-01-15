jQuery(document).ready(function ($) {
    $(".skill-name").on("change", function () {
        let newValue = $(this).val();
        $(this).attr("value", newValue);

        let updateSkill = "#update-" + $(this).attr("id");
        $(updateSkill).attr("value", newValue);
    });
    // $(".edit-btn").on("click", function (event) {
    //     event.preventDefault();

    //     $(".skill-name").prop("disabled", true);
    //     let skill_input = "#" + $(this).attr("data-skill");
    //     $(skill_input).removeAttr("disabled").focus();
    //     var length = $(skill_input).val().length;
    //     $(skill_input)[0].setSelectionRange(length, length);
    // });

    $(document).on("click", ".delete-skill", function () {
        let skill_id = $(this).attr("data-skill-id");
        try {
            $.ajax({
                url: "/Skill/GetSkillDeleteModal",
                type: "POST",
                data: { skill_id: skill_id },
                success: function (response) {
                    console.log(response);
                    $("#deleteSkillModal").html(response);
                    $("#deleteSkillModal .modal").modal("show");
                },
            });
        } catch (error) {
            console.log(error);
        }
    });
});
