/**
 * Generated by orval v6.16.0 🍺
 * Do not edit manually.
 * Blueboard
 * OpenAPI spec version: v4.0.0
 */
import type { ShopViewOwnedItemResponseProduct } from './shopViewOwnedItemResponseProduct';

export interface ShopViewOwnedItemResponse {
  id?: number;
  userId?: string;
  product?: ShopViewOwnedItemResponseProduct;
  usedAt?: string | null;
  createdAt?: string;
  updatedAt?: string;
}