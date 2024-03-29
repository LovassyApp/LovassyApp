/**
 * Generated by orval v6.17.0 🍺
 * Do not edit manually.
 * Blueboard
 * OpenAPI spec version: 4.1.0
 */
import {
  useQuery,
  useMutation
} from '@tanstack/react-query'
import type {
  UseQueryOptions,
  UseMutationOptions,
  QueryFunction,
  MutationFunction,
  UseQueryResult,
  QueryKey
} from '@tanstack/react-query'
import type {
  StatusViewVersionResponse,
  ProblemDetails,
  GetApiStatusVersionParams,
  StatusViewServiceStatusResponse,
  StatusNotifyOnResetKeyPasswordSetRequestBody
} from '../../models'
import { useCustomClient } from '../../../customClient';
import type { ErrorType, BodyType } from '../../../customClient';

type AwaitedInput<T> = PromiseLike<T> | T;

      type Awaited<O> = O extends AwaitedInput<infer T> ? T : never;


/**
 * @summary Get information about the application version
 */
export const getApiStatusVersion = (
    params: GetApiStatusVersionParams,
 signal?: AbortSignal
) => {
      return useCustomClient<StatusViewVersionResponse>(
      {url: `/Api/Status/Version`, method: 'get',
        params, signal
    },
      );
    }
  

export const getGetApiStatusVersionQueryKey = (params: GetApiStatusVersionParams,) => [`/Api/Status/Version`, ...(params ? [params]: [])] as const;
  

    
export const getGetApiStatusVersionQueryOptions = <TData = Awaited<ReturnType<typeof getApiStatusVersion>>, TError = ErrorType<ProblemDetails>>(params: GetApiStatusVersionParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiStatusVersion>>, TError, TData>, }
): UseQueryOptions<Awaited<ReturnType<typeof getApiStatusVersion>>, TError, TData> & { queryKey: QueryKey } => {
const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getGetApiStatusVersionQueryKey(params);

  
  
    const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiStatusVersion>>> = ({ signal }) => getApiStatusVersion(params, signal);
    
      
      
   return  { queryKey, queryFn, ...queryOptions}}

export type GetApiStatusVersionQueryResult = NonNullable<Awaited<ReturnType<typeof getApiStatusVersion>>>
export type GetApiStatusVersionQueryError = ErrorType<ProblemDetails>

/**
 * @summary Get information about the application version
 */
export const useGetApiStatusVersion = <TData = Awaited<ReturnType<typeof getApiStatusVersion>>, TError = ErrorType<ProblemDetails>>(
 params: GetApiStatusVersionParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiStatusVersion>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const queryOptions = getGetApiStatusVersionQueryOptions(params,options)

  const query = useQuery(queryOptions) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryOptions.queryKey ;

  return query;
}

/**
 * @summary Get information about the status of the application
 */
export const getApiStatusServiceStatus = (
    
 signal?: AbortSignal
) => {
      return useCustomClient<StatusViewServiceStatusResponse>(
      {url: `/Api/Status/ServiceStatus`, method: 'get', signal
    },
      );
    }
  

export const getGetApiStatusServiceStatusQueryKey = () => [`/Api/Status/ServiceStatus`] as const;
  

    
export const getGetApiStatusServiceStatusQueryOptions = <TData = Awaited<ReturnType<typeof getApiStatusServiceStatus>>, TError = ErrorType<unknown>>( options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiStatusServiceStatus>>, TError, TData>, }
): UseQueryOptions<Awaited<ReturnType<typeof getApiStatusServiceStatus>>, TError, TData> & { queryKey: QueryKey } => {
const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getGetApiStatusServiceStatusQueryKey();

  
  
    const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiStatusServiceStatus>>> = ({ signal }) => getApiStatusServiceStatus(signal);
    
      
      
   return  { queryKey, queryFn, ...queryOptions}}

export type GetApiStatusServiceStatusQueryResult = NonNullable<Awaited<ReturnType<typeof getApiStatusServiceStatus>>>
export type GetApiStatusServiceStatusQueryError = ErrorType<unknown>

/**
 * @summary Get information about the status of the application
 */
export const useGetApiStatusServiceStatus = <TData = Awaited<ReturnType<typeof getApiStatusServiceStatus>>, TError = ErrorType<unknown>>(
  options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiStatusServiceStatus>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const queryOptions = getGetApiStatusServiceStatusQueryOptions(options)

  const query = useQuery(queryOptions) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryOptions.queryKey ;

  return query;
}

/**
 * @summary Subscribe an email to when a password reset key has been set
 */
export const postApiStatusNotifyOnResetKeyPasswordSet = (
    statusNotifyOnResetKeyPasswordSetRequestBody: BodyType<StatusNotifyOnResetKeyPasswordSetRequestBody>,
 ) => {
      return useCustomClient<void>(
      {url: `/Api/Status/NotifyOnResetKeyPasswordSet`, method: 'post',
      headers: {'Content-Type': 'application/json', },
      data: statusNotifyOnResetKeyPasswordSetRequestBody
    },
      );
    }
  


export const getPostApiStatusNotifyOnResetKeyPasswordSetMutationOptions = <TError = ErrorType<unknown>,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof postApiStatusNotifyOnResetKeyPasswordSet>>, TError,{data: BodyType<StatusNotifyOnResetKeyPasswordSetRequestBody>}, TContext>, }
): UseMutationOptions<Awaited<ReturnType<typeof postApiStatusNotifyOnResetKeyPasswordSet>>, TError,{data: BodyType<StatusNotifyOnResetKeyPasswordSetRequestBody>}, TContext> => {
 const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof postApiStatusNotifyOnResetKeyPasswordSet>>, {data: BodyType<StatusNotifyOnResetKeyPasswordSetRequestBody>}> = (props) => {
          const {data} = props ?? {};

          return  postApiStatusNotifyOnResetKeyPasswordSet(data,)
        }

        

 
   return  { mutationFn, ...mutationOptions }}

    export type PostApiStatusNotifyOnResetKeyPasswordSetMutationResult = NonNullable<Awaited<ReturnType<typeof postApiStatusNotifyOnResetKeyPasswordSet>>>
    export type PostApiStatusNotifyOnResetKeyPasswordSetMutationBody = BodyType<StatusNotifyOnResetKeyPasswordSetRequestBody>
    export type PostApiStatusNotifyOnResetKeyPasswordSetMutationError = ErrorType<unknown>

    /**
 * @summary Subscribe an email to when a password reset key has been set
 */
export const usePostApiStatusNotifyOnResetKeyPasswordSet = <TError = ErrorType<unknown>,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof postApiStatusNotifyOnResetKeyPasswordSet>>, TError,{data: BodyType<StatusNotifyOnResetKeyPasswordSetRequestBody>}, TContext>, }
) => {
    
      const mutationOptions = getPostApiStatusNotifyOnResetKeyPasswordSetMutationOptions(options);
     
      return useMutation(mutationOptions);
    }
    