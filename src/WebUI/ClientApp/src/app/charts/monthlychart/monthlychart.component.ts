import { Component } from '@angular/core';
import * as Highcharts from 'highcharts';
import { TransactionClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-monthlychart',
  templateUrl: './monthlychart.component.html',
  styleUrls: ['./monthlychart.component.scss']
})
export class MonthlychartComponent {
  chart: any;
  currentUserId: string = '';
  selectedCategory: string = 'totalExpense'; // Default selection

  constructor(private transactionClient:TransactionClient) {
    const currentUser = sessionStorage.getItem('currentUser');
    if (currentUser) {
      const parsedUser = JSON.parse(currentUser);
      this.currentUserId = parsedUser.id;
    }
    this.selectedCategory = 'all'; 
    
  }

  ngOnInit() {
    this.chart = Highcharts.chart('container', this.options);
    this.getAllExpense();
  }

  // Highcharts options for the bar chart
  options: any = {
    chart: {
      type: 'bar',
    },
    title: {
      text: 'Monthly Financial Data',
    },
    xAxis: {
      categories: [], // This will be updated dynamically
      title: {
        text: 'Transaction Categories',
      },
    },
    yAxis: {
      min: 0,
      title: {
        text: 'Amount in Rs',
        align: 'high',
      },
      labels: {
        overflow: 'justify',
      },
    },
    tooltip: {
      valuePrefix: 'Rs. ',
    },
    plotOptions: {
      bar: {
        dataLabels: {
          enabled: true,
        },
      },
    },
    credits: {
      enabled: false,
    },
    series: [
      {
        name: 'Amount',
        data: [], // This will be updated dynamically
      },
    ],
  };

  // Method to fetch all transactions and update the chart
  getAllExpense() {
    debugger
    this.transactionClient.getAll(this.currentUserId)
      .subscribe(
        (response) => {
          this.updateChart(response.thisMonth);
          console.log(response); // Update chart based on the data received
        },
        (error) => {
          console.error('Error fetching data:', error);
        }
      );
  }

  // Method triggered when the user selects a different category
  onSelectionChange(selectedCategory: string) {
    debugger
    this.selectedCategory = selectedCategory;
    this.getAllExpense(); // Re-fetch data and update the chart when the selection changes
  }

  // Method to update the chart based on the selected category
  updateChart(data: any) {
    // Initialize variables
    let categories: string[] = [];
    let chartData: number[] = [];
    
    // Define categories and data based on selected option
    if (this.selectedCategory === 'totalExpense') {
      categories = ['Total Expense'];
      chartData = [data.totalExpense];
    } else if (this.selectedCategory === 'totalIncome') {
      categories = ['Total Income'];
      chartData = [data.totalIncome];
    } else if (this.selectedCategory === 'all') {
      // Show both total expense and total income
      categories = ['Total Expense', 'Total Income'];
      chartData = [data.totalExpense, data.totalIncome];
    }
  
    // Update chart
    this.chart.xAxis[0].setCategories(categories);
    this.chart.series[0].setData(chartData);
  }
}
