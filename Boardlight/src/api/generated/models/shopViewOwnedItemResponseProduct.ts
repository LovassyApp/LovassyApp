/**
 * Generated by orval v6.17.0 🍺
 * Do not edit manually.
 * Blueboard
 * OpenAPI spec version: 4.1.0
 */
import type { ShopViewOwnedItemResponseProductInput } from './shopViewOwnedItemResponseProductInput';

export interface ShopViewOwnedItemResponseProduct {
  id?: number;
  name?: string | null;
  description?: string | null;
  richTextContent?: string | null;
  qrCodeActivated?: boolean;
  inputs?: ShopViewOwnedItemResponseProductInput[] | null;
  thumbnailUrl?: string | null;
  createdAt?: string;
  updatedAt?: string;
}
