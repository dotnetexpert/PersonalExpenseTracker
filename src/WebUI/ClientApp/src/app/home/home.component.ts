import { Component, EventEmitter, Input, Output } from '@angular/core';
import * as Highcharts from 'highcharts';
import { TransactionClient } from '../web-api-client';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent {
  @Input() isExpanded: boolean = false;
  @Output() toggleSidebar: EventEmitter<boolean> = new EventEmitter<boolean>();
  showFive: boolean = true;

  handleSidebarToggle = () => this.toggleSidebar.emit(!this.isExpanded);
  currentUserId: string = '';
  data: { [key: string]: any } = {}; // You may not need this anymore
  ExpenseList: any = {
    today: { transactions: [] },
    last7Days: { transactions: [] },
    thisMonth: { transactions: [] },
    thisYear: { transactions: [] },
  };
  Highcharts: typeof Highcharts = Highcharts;
  chartOptions: Highcharts.Options = {};
  selectedPeriod: string = 'today';

  constructor(private transactionClient: TransactionClient) {
    const currentUser = sessionStorage.getItem('currentUser');
    if (currentUser) {
      const parsedUser = JSON.parse(currentUser);
      this.currentUserId = parsedUser.id;
    }
  }

  ngOnInit() {
    this.getAllExpense();
  }
  transaction: any;
  updateChartData() {
    const transactions =
      this.ExpenseList[this.selectedPeriod]?.transactions || [];

    // Aggregate transactions by category, considering transactionType
    const categoryTotals = transactions.reduce(
      (acc: { [key: string]: number }, transaction: any) => {
        const categoryName = `${transaction.expenseCategory.name} (${transaction.transactionType})`;
        if (!acc[categoryName]) {
          acc[categoryName] = 0;
        }
        acc[categoryName] += transaction.expenseAmount;
        return acc;
      },
      {}
    );

    // Format data for the Highcharts pie chart
    const dataPoints = Object.keys(categoryTotals).map((category) => ({
      name: category,
      y: categoryTotals[category],
    }));

    console.log("transactions",transactions)

    console.log('Data Points:', dataPoints);

    // Update the chart options
    this.chartOptions = {
      chart: {
        type: 'pie',
      },
      title: {
        text: 'Expense Breakdown by Category',
      },
      series: [
        {
          type: 'pie',
          name: 'Amount',
          data: dataPoints,
        },
      ],
      plotOptions: {
        pie: {
          dataLabels: {
            enabled: true,
            format: '{point.name}: {point.y:.2f}', // Format the labels as Category: Amount
          },
        },
      },
    };
  }

  today: any;
  month: any;
  year: any;
  week: any;
  getAllExpense() {
    this.transactionClient.getAll(this.currentUserId).subscribe(
      (response) => {
        this.ExpenseList = response;
        console.log('ExpenseList', this.ExpenseList);
        this.today = this.ExpenseList.today;
        this.month = this.ExpenseList.thisMonth;
        this.year = this.ExpenseList.thisYear;
        this.week = this.ExpenseList.last7Days;
        this.year.transactions = this.year.transactions.sort(
          (a: any, b: any) => b.transactionId - a.transactionId
        );
        this.updateChartData(); // Update the chart with new data
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
