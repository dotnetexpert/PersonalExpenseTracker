import { Component } from '@angular/core';
import * as Highcharts from 'highcharts';
import { TransactionClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-expensebycategory',
  templateUrl: './expensebycategory.component.html',
  styleUrls: ['./expensebycategory.component.scss']
})
export class ExpensebycategoryComponent {
  startdate: any;
  enddate: any = new Date().toISOString().split('T')[0];
  currentUserId: string = '';
  ExpenseList:any
  Highcharts: typeof Highcharts = Highcharts;
  chartOptions1: Highcharts.Options = {}; // Initialize with an empty object
 
 
  something: any;
  constructor(private transactionClient:TransactionClient) {
    const currentUser = sessionStorage.getItem('currentUser');
    if (currentUser) {
      const parsedUser = JSON.parse(currentUser);
      this.currentUserId = parsedUser.id;
    }
  }
  ngOnInit() {
    var todayDate = new Date();
    this.startdate = new Date(todayDate.getFullYear(), todayDate.getMonth(), 2).toISOString().split('T')[0];
    this.getTRansactionsByDate();
    this.updateChartOptions();
  }
 
  today: any;
  month: any;
  year: any;
  week: any;
  filteredExpenses: any[] = [];
  chartCategories: string[] = [];
  chartData: number[] = [];

  getTRansactionsByDate(){
    this.transactionClient.getTodayTransactions(this.currentUserId,new Date(this.startdate),new Date(this.enddate)).subscribe((response)=>{
      const transactions = response.transactions;
 
      // Filter out only expenses (transactionType: 'Debit')
      this.filteredExpenses = transactions.filter((transaction: any) => transaction.transactionType === 'Debit');

      // Group expenses by category
      const categoryMap = new Map<string, number>();

      this.filteredExpenses.forEach((transaction: any) => {
        const category = transaction.expenseCategory?.name || 'Uncategorized';
        const currentAmount = categoryMap.get(category) || 0;
        categoryMap.set(category, currentAmount + transaction.expenseAmount);
      });

      // Prepare data for the chart
      this.chartCategories = Array.from(categoryMap.keys());
      this.chartData = Array.from(categoryMap.values());

   // Update the chart options
    this.updateChartOptions();
    
      
    },(error)=>{
      console.log(error);
    })
  }

  
  updateChartOptions() {
    this.chartOptions1 = {
      chart: {
        type: 'column'
      },
      title: {
        text: 'Expense by Category',
        align: 'left'
      },
      xAxis: {
        categories: this.chartCategories,
        crosshair: true,
        accessibility: {
          description: 'Transaction dates'
        }
      },
      yAxis: {
        min: 0,
        title: {
          text: 'Expense by Category'
        }
      },
      tooltip: {
        valueSuffix: ' (Amount)'
      },
      plotOptions: {
        column: {
          pointPadding: 0.2,
          borderWidth: 0
        }
      },
      series: [
        {
          type: 'column',
          name: 'Expenses',
          data: this.chartData
        }
      ]
    };
  }



}
