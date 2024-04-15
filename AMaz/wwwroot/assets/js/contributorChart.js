async function fetchAndCreateContributorCountChart() {
    try {
        const response = await fetch('/Dashboard/GetContributorCount');
        const { FirstLabels, SecondLabels, Data } = await response.json();

        // Ensure the response contains valid data
        if (!FirstLabels || !SecondLabels || !Data) {
            console.error('Invalid data received from the server');
            return;
        }

        const academicYears = FirstLabels;
        const faculties = SecondLabels;

        // Prepare the dataset for the chart
        const dataset = faculties.map((faculty, index) => ({
            label: faculty,
            data: Data.map(row => row[index]), // Extract data for each faculty
            backgroundColor: getRandomColor(),
            borderColor: '#fff',
            borderWidth: 1
        }));

        const ctx = document.getElementById('contributorCountChart').getContext('2d');
        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: academicYears,
                datasets: dataset
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    } catch (error) {
        console.error('Error fetching and creating contributor count chart:', error);
    }
}

document.addEventListener('DOMContentLoaded', fetchAndCreateContributorCountChart);
