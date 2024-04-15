async function fetchAndCreateContributionChart() {
    try {
        const response = await fetch('/Dashboard/GetContributionCount');
        const { FirstLabels, SecondLabels, Data } = await response.json();

        // Assuming FirstLabels contain academic years and SecondLabels contain faculty names
        const academicYears = FirstLabels;
        const faculties = SecondLabels;

        // Transpose the data to match the expected structure for the chart
        const transposedData = academicYears.map((year, index) => ({
            [year]: Object.fromEntries(faculties.map((faculty, i) => [faculty, Data[i][index]]))
        })).reduce((acc, obj) => {
            for (const key in obj) {
                if (!acc[key]) {
                    acc[key] = {};
                }
                Object.assign(acc[key], obj[key]);
            }
            return acc;
        }, {});

        // Prepare the dataset for the chart
        const dataset = faculties.map(faculty => ({
            label: faculty,
            data: academicYears.map(year => transposedData[year][faculty] || 0),
            backgroundColor: getRandomColor(),
            borderColor: getRandomColor(),
            borderWidth: 1
        }));

        const ctx = document.getElementById('contributionChart').getContext('2d');
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
                }
            }
        });
    } catch (error) {
        console.error('Error fetching and creating contribution chart:', error);
    }
}

// Function to get random color
function getRandomColor() {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

document.addEventListener('DOMContentLoaded', fetchAndCreateContributionChart);
