import { Component } from '@angular/core';
import * as Highcharts from 'highcharts';
import { TransactionClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-categorychart',
  templateUrl: './categorychart.component.html',
  styleUrls: ['./categorychart.component.scss']
})
export class CategorychartComponent {
  chart: any;
  currentUserId: string = '';
  selectedCategory: string = 'today'; // Default selection

  constructor(private transactionClient:TransactionClient) {
    const currentUser = sessionStorage.getItem('currentUser');
    if (currentUser) {
      const parsedUser = JSON.parse(currentUser);
      this.currentUserId = parsedUser.id;
    }
    this.selectedCategory = 'today';
  }

  ngOnInit() {
    this.chart = Highcharts.chart('container1', this.options);
    this.getAllExpense();
  }

  // Highcharts options for the bar chart
  options: any = {
    chart: {
      type: 'bar',
    },
    title: {
      text: 'Transaction Categories',
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

  getAllExpense() {
    debugger;
    this.transactionClient.getAll(this.currentUserId)
      .subscribe(
        (response) => {
          // Update chart based on the selected category
          switch (this.selectedCategory) {
            case 'last7Days':
              this.updateChart(response.last7Days);
              break;
            case 'today':
              this.updateChart(response.today);
              break;
            case 'thisMonth':
              this.updateChart(response.thisMonth);
              break;
            case 'thisYear':
              this.updateChart(response.thisYear);
              break;
          }
          console.log(response.last7Days); // For debugging purposes
        },
        (error) => {
          console.error('Error fetching data:', error);
        }
      );
  }

  // Method triggered when the user selects a different category
  onSelectionChange(selectedCategory: string) {
    debugger;
    this.selectedCategory = selectedCategory;
    this.getAllExpense(); // Re-fetch data and update the chart when the selection changes
  }
  updateChart(data: any) {
    // Initialize variables
    let categories: string[] = [];
    let chartData: any[] = [];

    // Filter transactions based on selected option
    const transactions = data.transactions.filter((t: any) => {
      if (this.selectedCategory === 'today') return true;
      if (this.selectedCategory === 'last7Days') return true;
      if (this.selectedCategory === 'thisMonth') return true;
      if (this.selectedCategory === 'thisYear') return true;
      return false; // Default case, filter nothing
    });

    // Create a map to aggregate data
    const categoryMap = new Map<string, { amount: number; type: string }>();

    transactions.forEach((t: any) => {
      const key = `${t.expenseCategory.name} - ${t.transactionType}`;
      if (!categoryMap.has(key)) {
        categoryMap.set(key, { amount: 0, type: t.transactionType });
      }
      const existing = categoryMap.get(key)!;
      existing.amount += t.expenseAmount;
    });

    // Populate categories and chartData arrays
    categories = Array.from(categoryMap.keys());
    chartData = Array.from(categoryMap.values()).map(item => ({
      y: item.amount,
      // color: item.type === 'Debit' ? 'red' : 'green' // Set colors based on transaction type
    }));

    // Update chart
    this.chart.xAxis[0].setCategories(categories);
    this.chart.series[0].setData(chartData);
  }
}
