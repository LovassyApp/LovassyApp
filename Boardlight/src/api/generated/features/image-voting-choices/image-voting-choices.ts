/**
 * Generated by orval v6.17.0 🍺
 * Do not edit manually.
 * Blueboard
 * OpenAPI spec version: 4.1.0
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
  ImageVotingsIndexImageVotingChoicesResponse,
  GetApiImageVotingChoicesParams,
  ImageVotingsViewImageVotingChoiceResponse,
  ProblemDetails
} from '../../models'
import { useCustomClient } from '../../../customClient';
import type { ErrorType } from '../../../customClient';

type AwaitedInput<T> = PromiseLike<T> | T;

      type Awaited<O> = O extends AwaitedInput<infer T> ? T : never;


/**
 * Requires verified email; Requires one of the following permissions: ImageVotings.IndexImageVotingChoices, ImageVotings.IndexActiveImageVotingChoices; Requires the following features to be enabled: ImageVotings
 * @summary Get a list of all image voting choices
 */
export const getApiImageVotingChoices = (
    params?: GetApiImageVotingChoicesParams,
 signal?: AbortSignal
) => {
      return useCustomClient<ImageVotingsIndexImageVotingChoicesResponse[]>(
      {url: `/Api/ImageVotingChoices`, method: 'get',
        params, signal
    },
      );
    }
  

export const getGetApiImageVotingChoicesQueryKey = (params?: GetApiImageVotingChoicesParams,) => [`/Api/ImageVotingChoices`, ...(params ? [params]: [])] as const;
  

    
export const getGetApiImageVotingChoicesQueryOptions = <TData = Awaited<ReturnType<typeof getApiImageVotingChoices>>, TError = ErrorType<void>>(params?: GetApiImageVotingChoicesParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiImageVotingChoices>>, TError, TData>, }
): UseQueryOptions<Awaited<ReturnType<typeof getApiImageVotingChoices>>, TError, TData> & { queryKey: QueryKey } => {
const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getGetApiImageVotingChoicesQueryKey(params);

  
  
    const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiImageVotingChoices>>> = ({ signal }) => getApiImageVotingChoices(params, signal);
    
      
      
   return  { queryKey, queryFn, ...queryOptions}}

export type GetApiImageVotingChoicesQueryResult = NonNullable<Awaited<ReturnType<typeof getApiImageVotingChoices>>>
export type GetApiImageVotingChoicesQueryError = ErrorType<void>

/**
 * @summary Get a list of all image voting choices
 */
export const useGetApiImageVotingChoices = <TData = Awaited<ReturnType<typeof getApiImageVotingChoices>>, TError = ErrorType<void>>(
 params?: GetApiImageVotingChoicesParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiImageVotingChoices>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const queryOptions = getGetApiImageVotingChoicesQueryOptions(params,options)

  const query = useQuery(queryOptions) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryOptions.queryKey ;

  return query;
}

/**
 * Requires verified email; Requires one of the following permissions: ImageVotings.ViewImageVotingChoice, ImageVotings.ViewActiveImageVotingChoice; Requires the following features to be enabled: ImageVotings
 * @summary Get information about an image voting choice
 */
export const getApiImageVotingChoicesId = (
    id: number,
 signal?: AbortSignal
) => {
      return useCustomClient<ImageVotingsViewImageVotingChoiceResponse>(
      {url: `/Api/ImageVotingChoices/${id}`, method: 'get', signal
    },
      );
    }
  

export const getGetApiImageVotingChoicesIdQueryKey = (id: number,) => [`/Api/ImageVotingChoices/${id}`] as const;
  

    
export const getGetApiImageVotingChoicesIdQueryOptions = <TData = Awaited<ReturnType<typeof getApiImageVotingChoicesId>>, TError = ErrorType<void | ProblemDetails>>(id: number, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiImageVotingChoicesId>>, TError, TData>, }
): UseQueryOptions<Awaited<ReturnType<typeof getApiImageVotingChoicesId>>, TError, TData> & { queryKey: QueryKey } => {
const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getGetApiImageVotingChoicesIdQueryKey(id);

  
  
    const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiImageVotingChoicesId>>> = ({ signal }) => getApiImageVotingChoicesId(id, signal);
    
      
      
   return  { queryKey, queryFn, enabled: !!(id), ...queryOptions}}

export type GetApiImageVotingChoicesIdQueryResult = NonNullable<Awaited<ReturnType<typeof getApiImageVotingChoicesId>>>
export type GetApiImageVotingChoicesIdQueryError = ErrorType<void | ProblemDetails>

/**
 * @summary Get information about an image voting choice
 */
export const useGetApiImageVotingChoicesId = <TData = Awaited<ReturnType<typeof getApiImageVotingChoicesId>>, TError = ErrorType<void | ProblemDetails>>(
 id: number, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiImageVotingChoicesId>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const queryOptions = getGetApiImageVotingChoicesIdQueryOptions(id,options)

  const query = useQuery(queryOptions) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryOptions.queryKey ;

  return query;
}

