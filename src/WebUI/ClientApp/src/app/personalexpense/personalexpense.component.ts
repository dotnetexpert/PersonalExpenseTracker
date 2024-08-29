import {
  PayeeListClient,
  PaymentMethodClient,
  ExpenseCategoryClient,
  ExpenseCategory,
  PayeeList,
  PaymentMethod,
  TransactionClient,
  Transaction,
} from './../web-api-client';
import { ToastrService } from 'ngx-toastr';
import { Component } from '@angular/core';
import { TransactionType } from '../personalexpense/enum';

@Component({
  selector: 'app-personalexpense',
  templateUrl: './personalexpense.component.html',
  styleUrls: ['./personalexpense.component.scss'],
})
export class PersonalexpenseComponent {
  name: any;
  currentUserId: string = '';
  categoryId: number = 0;
  paymentMethodId: number = 0;
  payeeId: number = 0;
  description: string = '';
  expenseAmount: number = 0;
  transactionDate: any = new Date().toISOString().split('T')[0];

  payeelist: any;
  paymentMethodlist: any;
  expenseCatagorylist: any;
  transactionType: boolean = true;
  constructor(
    private toastrService: ToastrService,
    private payeeListClient: PayeeListClient,
    private paymentMethodClient: PaymentMethodClient,
    private expenseCategoryClient: ExpenseCategoryClient,
    private transactionClient: TransactionClient
  ) {
    const currentUser = sessionStorage.getItem('currentUser');
    if (currentUser) {
      const parsedUser = JSON.parse(currentUser);
      this.currentUserId = parsedUser.id;
    }
  }

  ngOnInit() {
    this.getAllPayList();
    this.getAllPaymentMethodList();
    this.expenseCatagoryList();
  }

  // Transaction Part

  transaction: Transaction = new Transaction();
  createTransaction() {
    (this.transaction.expenseAmount = this.expenseAmount),
      (this.transaction.applicationUserId = this.currentUserId),
      (this.transaction.categoryId = this.categoryId),
      (this.transaction.paymentMethodId = this.paymentMethodId),
      (this.transaction.payeeId = this.payeeId),
      (this.transaction.description = this.description),
      (this.transaction.transactionDate = new Date(this.transactionDate)),
      (this.transaction.transactionTypeId = this.transactionType
        ? TransactionType.Debit
        : TransactionType.Credit),
      this.transactionClient.upsert(this.transaction).subscribe(
        (response) => {
          this.toastrService.success('Save expense successfully', 'success');
          this.clearRecord();
        },
        (error) => {
          console.error('Transaction failed:', error);
        }
      );
  }

  // Expense Part

  expenseCatagoryList() {
    this.expenseCategoryClient
      .getAll(this.currentUserId, this.transactionType ? 0 : 1)
      .subscribe(
        (response) => {
          this.expenseCatagorylist = response.expenseCategories;
        },
        (error) => {
          console.log(error);
        }
      );
  }

  expenseCategory: ExpenseCategory = new ExpenseCategory();

  createExpenseCategory() {
    this.expenseCategory.applicationUserId = this.currentUserId;
    this.expenseCategory.name = this.name;
    this.expenseCategory.isDefault = false;
    this.expenseCategory.isDeleted = false;
    (this.expenseCategory.transactionTypeId = this.transactionType
      ? TransactionType.Debit
      : TransactionType.Credit),
      this.expenseCategoryClient.upsert(this.expenseCategory).subscribe(
        (response) => {
          this.toastrService.success('Save expense successfully', 'success');
          this.expenseCatagoryList();
          this.clearRec();
        },
        (error) => {
          console.log(error);
        }
      );
  }

  deleteExpenseById(id: any) {
    this.expenseCategoryClient.delete(id).subscribe(
      (response) => {
        this.expenseCatagoryList();
        this.toastrService.success('Deleted  successfully.', 'success');
      },

      (error) => {
        this.toastrService.error('Failed to Deleted the data.', 'error');
      }
    );
  }

  // Payment Method Part

  getAllPaymentMethodList() {
    this.paymentMethodClient.getAll(this.currentUserId).subscribe(
      (response) => {
        this.paymentMethodlist = response.paymentMethods;
      },
      (error) => {
        console.log(error);
      }
    );
  }

  paymentMethod: PaymentMethod = new PaymentMethod();
  createPayementMethod() {
    this.paymentMethod.name = this.name;
    this.paymentMethod.isDefault = false;
    this.paymentMethod.isDeleted = false;
    this.paymentMethod.applicationUserId = this.currentUserId;
    this.paymentMethodClient.upsert(this.paymentMethod).subscribe(
      (response) => {
        this.toastrService.success(
          'Save PayementMethod successfully',
          'success'
        );
        this.getAllPaymentMethodList();
        this.clearRec();
      },
      (error) => {
        console.log(error);
      }
    );
  }

  deletePaymentMethodById(id: any) {
    this.paymentMethodClient.delete(id).subscribe(
      (response) => {
        this.getAllPaymentMethodList();
        this.toastrService.success('Deleted  successfully.', 'success');
      },

      (error) => {
        this.toastrService.error('Failed to Deleted the data.', 'error');
      }
    );
  }

  // Payee Part

  getAllPayList() {
    this.payeeListClient
      .getAll(
        this.currentUserId,
        this.transactionType ? TransactionType.Debit : TransactionType.Credit
      )
      .subscribe(
        (response) => {
          this.payeelist = response.payeeLists;
        },
        (error) => {
          console.log(error);
        }
      );
  }

  payeeList: PayeeList = new PayeeList();
  createPayee() {
    this.payeeList.applicationUserId = this.currentUserId;
    this.payeeList.isDefault = false;
    this.payeeList.isDefault = false;
    this.payeeList.name = this.name;
    (this.payeeList.transactionTypeId = this.transactionType
      ? TransactionType.Debit
      : TransactionType.Credit),
      this.payeeListClient.upsert(this.payeeList).subscribe(
        (response) => {
          this.toastrService.success('Save Payee successfully', 'success');
          this.getAllPayList();
          this.clearRec();
        },
        (error) => {
          console.log(error);
        }
      );
  }

  deletePayeeById(id: any) {
    this.payeeListClient.delete(id).subscribe(
      (response) => {
        this.getAllPayList();
        this.toastrService.success('Deleted  successfully.', 'success');
      },

      (error) => {
        this.toastrService.error('Failed to Deleted the data.', 'error');
      }
    );
  }

  // Clear Part

  clearRec() {
    this.name = '';
  }

  clearRecord() {
    this.categoryId = 0;
    this.paymentMethodId = 0;
    this.payeeId = 0;
    this.description = '';
    this.expenseAmount = 0;
    this.transactionDate = new Date(); // Set to an empty string to clear the date
  }
}
