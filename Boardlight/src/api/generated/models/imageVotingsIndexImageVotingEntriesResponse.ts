/**
 * Generated by orval v6.17.0 🍺
 * Do not edit manually.
 * Blueboard
 * OpenAPI spec version: v4.0.0
 */
import type { ImageVotingsIndexImageVotingEntriesResponseUser } from './imageVotingsIndexImageVotingEntriesResponseUser';

export interface ImageVotingsIndexImageVotingEntriesResponse {
  id?: number;
  title?: string | null;
  imageUrl?: string | null;
  userId?: string | null;
  user?: ImageVotingsIndexImageVotingEntriesResponseUser;
  imageVotingId?: number;
  canChoose?: boolean | null;
  chosen?: boolean | null;
  incrementType?: string | null;
  createdAt?: string;
  updatedAt?: string;
}
