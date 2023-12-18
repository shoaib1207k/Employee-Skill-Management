jQuery(document).ready(function($){

    $('.skill-name').on('change', function(){
        let newValue = $(this).val();
        $(this).attr('value',newValue);

        let updateSkill = '#update-'+$(this).attr('id');
        $(updateSkill).attr('value',newValue);
    })
    $('.edit-btn').on('click',function(event){
        event.preventDefault();

        $('.skill-name').prop('disabled', true);
        let skill_input = '#'+$(this).attr('data-skill');
        $(skill_input).removeAttr('disabled').focus();
        var length = $(skill_input).val().length;
        $(skill_input)[0].setSelectionRange(length, length);
    })
})