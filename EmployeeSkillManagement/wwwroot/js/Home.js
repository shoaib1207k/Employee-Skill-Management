jQuery(document).ready(function($){
    $(document).ready(function () {

        let homepage = $(".homepage");
        if(homepage.length>0){
            console.log(homepage)
            $.ajax({
                url: 'Home/Index', // Replace with your actual endpoint
                method: 'GET',
                dataType: 'json',
                success: function (data) {
                    skillChartData = data.skillWithEmployeeCount;
                    const labels = Object.keys(skillChartData);
                    const values = Object.values(skillChartData);
                    const backgroundColors = generateColors(labels.length);

                    const ctx = document.getElementById('skillPieChart').getContext('2d');
                    const myPieChart = new Chart(ctx, {
                        type: 'pie',
                        data: {
                            labels: labels,
                            datasets: [{
                                data: values,
                                backgroundColor: backgroundColors
                            }],
                        },
                    });
                },
                error: function (error) {
                    console.error('Error fetching data:', error);
                }
            });    
        }
    });

    function generateColors(numColors) {
        const colors = [];
        for (let i = 0; i < numColors; i++) {
            const hue = (i * 360) / numColors; // Distribute hues evenly
            const color = `hsl(${hue}, 70%, 60%)`; // Adjust saturation and lightness as needed
            colors.push(color);
        }
        return colors;
    }
    
})