/**
 * Generated by orval v6.17.0 🍺
 * Do not edit manually.
 * Blueboard
 * OpenAPI spec version: v4.0.0
 */
import type { ShopIndexOwnLolosResponseGrade } from './shopIndexOwnLolosResponseGrade';

export interface ShopIndexOwnLolosResponseCoin {
  id?: number;
  userId?: string;
  isSpent?: boolean;
  loloType?: string | null;
  reason?: string | null;
  grades?: ShopIndexOwnLolosResponseGrade[] | null;
  createdAt?: string;
  updatedAt?: string;
}
