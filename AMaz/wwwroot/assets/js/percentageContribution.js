async function fetchAndCreatePercentageChart() {
    try {
        const response = await fetch('/Dashboard/GetFacultyContributorPercentage');
        if (!response.ok) {
            throw new Error('Failed to fetch data');
        }
        const { FirstLabels, SecondLabels, Data } = await response.json();

        const academicYears = FirstLabels;
        const faculties = SecondLabels || ["Faculty"]; // Default label if SecondLabels is null
        const data = Data;

        const datasets = faculties.map((faculty, index) => ({
            label: faculty,
            data: data.map(item => item[index]),
            backgroundColor: getRandomColor(index),
            borderColor: '#fff',
            borderWidth: 1
        }));

        const ctx = document.getElementById('percentageChart').getContext('2d');
        new Chart(ctx, {
            type: 'pie',
            data: {
                labels: academicYears,
                datasets: datasets
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                layout: {
                    padding: {
                        left: 5,
                        right: 5,
                        top: 5,
                        bottom: 5
                    }
                },
                plugins: {
                    legend: {
                        position: 'bottom'
                    }
                }
            }
        });
    } catch (error) {
        console.error('Error fetching and creating percentage chart:', error);
    }
}

function getRandomColor(index) {
    const colors = [
        'rgba(255, 99, 132, 0.8)',
        'rgba(54, 162, 235, 0.8)',
        'rgba(255, 206, 86, 0.8)',
        'rgba(75, 192, 192, 0.8)',
        'rgba(153, 102, 255, 0.8)',
        'rgba(255, 159, 64, 0.8)',
        'rgba(255, 0, 255, 0.8)',
        'rgba(0, 255, 255, 0.8)',
        'rgba(128, 128, 128, 0.8)',
        'rgba(0, 128, 0, 0.8)',
        'rgba(255, 255, 0, 0.8)',
        'rgba(128, 0, 128, 0.8)',
        'rgba(0, 0, 255, 0.8)',
        'rgba(0, 0, 0, 0.8)'
    ];

    return colors[index % colors.length];
}

document.addEventListener('DOMContentLoaded', fetchAndCreatePercentageChart);
