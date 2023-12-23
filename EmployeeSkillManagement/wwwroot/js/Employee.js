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
                }
            });

        }catch (error) {
           console.log(error);
        }

    });

    let timeOut;
    $("#filterNameOrId").on("input", function(){
        clearTimeout(timeOut);
        timeOut = setTimeout(() => {
            employeesFilterAjax();
        }, 500);
    });

    $("#filterSkill").on("change", function(){
        employeesFilterAjax();
    });


    let reportTable = $('#generatedReport').DataTable( {
        dom: 'Bfrtip',
        buttons: [
            'excel', 'pdf'
        ],
        searching: false,
        pageLength: 15,
    });

    if($('#generatedReport').length>0){
        // Check the total number of records and apply styling
        if (reportTable.page.info().recordsDisplay <= 15) {
            $('.dataTables_paginate, .dataTables_info').hide();
        }

        // Listen for changes to the page length
        reportTable.on('length.dt', function (e, settings, len) {
            // Apply styling when the total number of records is less than 20
            if (table.page.info().recordsDisplay <= 15) {
                $('.dataTables_paginate, .dataTables_info').hide();
            } else {
                $('.dataTables_paginate, .dataTables_info').show();
            }
        });
    }


    function employeesFilterAjax(){
        let empNameOrId = $("#filterNameOrId").val();
        let skillId = $("#filterSkill").val();
       
        let filterData = {
            'empNameOrId': empNameOrId,
            'skillId': skillId
            }
        
        try {
            $.ajax({
                url: "Employee/SearchEmployee",
                type: "POST",
                data: filterData,
                success: function(response){
                    console.log(response)
                    $("#employeeListDiv").html(response);
                }
            });
        } catch (error) {
            console.log(error);
        }
    }
})