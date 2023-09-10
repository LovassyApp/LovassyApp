/**
 * Generated by orval v6.17.0 🍺
 * Do not edit manually.
 * Blueboard
 * OpenAPI spec version: v4.0.0
 */
import {
  useQuery
} from '@tanstack/react-query'
import type {
  UseQueryOptions,
  QueryFunction,
  UseQueryResult,
  QueryKey
} from '@tanstack/react-query'
import type {
  FeedIndexFeedItemsResponse,
  GetApiFeedItemsParams
} from '../../models'
import { useCustomClient } from '../../../customClient';
import type { ErrorType } from '../../../customClient';

type AwaitedInput<T> = PromiseLike<T> | T;

      type Awaited<O> = O extends AwaitedInput<infer T> ? T : never;


/**
 * Requires verified email; Requires one of the following permissions: Feed.IndexFeedItems
 * @summary Get a list of all feed items
 */
export const getApiFeedItems = (
    params?: GetApiFeedItemsParams,
 signal?: AbortSignal
) => {
      return useCustomClient<FeedIndexFeedItemsResponse[]>(
      {url: `/Api/FeedItems`, method: 'get',
        params, signal
    },
      );
    }
  

export const getGetApiFeedItemsQueryKey = (params?: GetApiFeedItemsParams,) => [`/Api/FeedItems`, ...(params ? [params]: [])] as const;
  

    
export const getGetApiFeedItemsQueryOptions = <TData = Awaited<ReturnType<typeof getApiFeedItems>>, TError = ErrorType<void>>(params?: GetApiFeedItemsParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiFeedItems>>, TError, TData>, }
): UseQueryOptions<Awaited<ReturnType<typeof getApiFeedItems>>, TError, TData> & { queryKey: QueryKey } => {
const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getGetApiFeedItemsQueryKey(params);

  
  
    const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiFeedItems>>> = ({ signal }) => getApiFeedItems(params, signal);
    
      
      
   return  { queryKey, queryFn, ...queryOptions}}

export type GetApiFeedItemsQueryResult = NonNullable<Awaited<ReturnType<typeof getApiFeedItems>>>
export type GetApiFeedItemsQueryError = ErrorType<void>

/**
 * @summary Get a list of all feed items
 */
export const useGetApiFeedItems = <TData = Awaited<ReturnType<typeof getApiFeedItems>>, TError = ErrorType<void>>(
 params?: GetApiFeedItemsParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiFeedItems>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const queryOptions = getGetApiFeedItemsQueryOptions(params,options)

  const query = useQuery(queryOptions) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryOptions.queryKey ;

  return query;
}

