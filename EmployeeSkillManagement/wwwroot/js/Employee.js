jQuery(document).ready(function($){
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
    })
})