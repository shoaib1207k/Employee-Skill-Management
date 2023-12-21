jQuery(document).ready(function($){

    // adding skill in create employee form
    $('#addSkill').on('click', function(event){
        event.preventDefault();
        // var selectedOptionText = ;
        let level = $('#levelSelect').val();
        let skillId = $("#SelectedSkillIds").val();
        let skill = $("#SelectedSkillIds").find("option[value='" + skillId + "']").text();
        console.log(skill);
        console.log(level);
        let skillIndex = $('.skill-level').length;

        if(skill!==null && level !==null){
            let skill_level =  `<div class="skill-level d-flex justify-content-around align-items-center border border-primary rounded col-3">`
                                    +skill+` Level-`+level+`
                                    <div class="btn "> 
                                        <i class="bi bi-x-circle btn-outline-danger rounded-circle"></i>
                                    </div>
                                    <input type="text" value="${skillId}" name="SkillIds[${skillIndex}]" hidden>
                                    <input type="text" value="${level}" name="SkillLevel[`+skillIndex+`]" hidden>

                                </div>`;
            $('#skillsContainer').prepend(skill_level);
        }else{
            alert("Please Select correct options")
        }
    });


    // Delete ajax function
    $(document).on('click', '.delete-employee', function(){
        let employee_id = $(this).attr('data-emp-id');
        console.log(employee_id);
        try {
            $.ajax({
                url: "/Employee/GetEmployeeDeleteModal",
                type: "POST",
                data: { employee_id: employee_id },
                success: function (response) {
                    console.log(response);
                    $("#deleteModal").html(response);
                    $("#deleteModal .modal").modal('show');
                },
                error: function () {
                    console.log(employee_id ," error");
                }
            });

        }catch (error) {
           console.log(error);
        }

    });
})