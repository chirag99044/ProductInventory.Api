import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Category, Product, ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent implements OnInit{
 id!: number;
  product: Product = { id: 0, name: '', price: 0, quantity: 0,categoryId: 0, created: '' };
  categories: Category[] = [];

  constructor(
    private service: ProductService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadCategories();
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    if(this.id) {
      this.service.getProduct(this.id).subscribe(res => this.product = res);
    }
  }

  save() {
    if(this.id) {
      this.service.updateProduct(this.id, this.product).subscribe(() => this.router.navigate(['/']));
    } else {
      this.service.addProduct(this.product).subscribe(() => this.router.navigate(['/']));
    }
  }

  loadCategories() {
    this.service.getCategories().subscribe(res => {
      this.categories = res;
    });
  }
  
  cancel() {
    this.router.navigate(['/']);
  }
}
