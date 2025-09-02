import { CommonModule, DecimalPipe } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Category, Product, ProductResponse, ProductService } from '../../services/product.service';
import { FormsModule } from '@angular/forms';
import { IgxGridComponent, IgxPaginatorComponent, IgxColumnComponent, IgxPaginatorModule, IgxGridModule, IgxSelectModule, IgxButtonDirective } from 'igniteui-angular';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule, RouterModule, FormsModule, IgxGridModule, IgxPaginatorModule, IgxGridComponent, IgxPaginatorComponent, IgxColumnComponent, DecimalPipe,IgxSelectModule,IgxButtonDirective],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent {
  products: Product[] = [];
  categories: Category[] = [];
  totalCount: number = 0;
  page: number = 1;
  pageSize: number = 10;
  Math = Math;
  @ViewChild('grid1', { static: true }) public grid1: IgxGridComponent | undefined;

  constructor(private productService: ProductService) { }

  ngOnInit() {
    this.loadCategories();
    this.loadProducts();
  }


  // ...existing code...
  loadProducts() {
    this.productService.getProducts(this.page + 1, this.pageSize).subscribe((res: ProductResponse) => {
      this.products = res.items;
      this.totalCount = res.totalCount;
    });
  }
  // ...existing code...
  delete(id: number) {
    if (confirm('Are you sure to delete?')) {
      this.productService.deleteProduct(id).subscribe(() => this.loadProducts());
    }
  }
  loadCategories() {
    this.productService.getCategories().subscribe(res => {
      this.categories = res;
    });
  }
  updateCategory(product: Product) {
    console.log("Category updated for product:", product.name);
  }

  nextPage() {
    if ((this.page * this.pageSize) < this.totalCount) {
      this.page++;
      this.loadProducts();
    }
  }
  onPageChange(newPage: number) {
    this.page = newPage;
    this.loadProducts();
  }
  onPageSizeChange(newSize: number) {
    this.pageSize = newSize;
    this.page = 0; // reset to first page when page size changes
    this.loadProducts();
  }
  prevPage() {
    if (this.page > 1) {
      this.page--;
      this.loadProducts();
    }
  }
}
