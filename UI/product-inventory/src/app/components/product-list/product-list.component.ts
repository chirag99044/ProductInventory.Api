import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Category, Product, ProductResponse, ProductService } from '../../services/product.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule, RouterModule,FormsModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent {
  products: Product[] = [];
  categories: Category  [] = [];
  totalCount: number = 0;
  constructor(private productService: ProductService) {}

  ngOnInit() {
    this.loadCategories();
    this.loadProducts();
  }


  // ...existing code...
loadProducts() {
  this.productService.getProducts().subscribe((res: ProductResponse) => {
    this.products = res.items;
    this.totalCount = res.totalCount;
  });
}
// ...existing code...
  delete(id: number) {
    if(confirm('Are you sure to delete?')) {
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
}
