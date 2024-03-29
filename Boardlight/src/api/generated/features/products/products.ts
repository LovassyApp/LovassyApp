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
  ShopIndexProductsResponse,
  GetApiProductsParams,
  ShopCreateProductResponse,
  ShopCreateProductRequestBody,
  ShopViewProductResponse,
  ProblemDetails,
  ShopUpdateProductRequestBody
} from '../../models'
import { useCustomClient } from '../../../customClient';
import type { ErrorType, BodyType } from '../../../customClient';

type AwaitedInput<T> = PromiseLike<T> | T;

      type Awaited<O> = O extends AwaitedInput<infer T> ? T : never;


/**
 * Requires verified email; Requires one of the following permissions: Shop.IndexProducts, Shop.IndexStoreProducts; Requires the following features to be enabled: Shop
 * @summary Get a list of all products or only store products depending on permissions
 */
export const getApiProducts = (
    params?: GetApiProductsParams,
 signal?: AbortSignal
) => {
      return useCustomClient<ShopIndexProductsResponse[]>(
      {url: `/Api/Products`, method: 'get',
        params, signal
    },
      );
    }
  

export const getGetApiProductsQueryKey = (params?: GetApiProductsParams,) => [`/Api/Products`, ...(params ? [params]: [])] as const;
  

    
export const getGetApiProductsQueryOptions = <TData = Awaited<ReturnType<typeof getApiProducts>>, TError = ErrorType<void>>(params?: GetApiProductsParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiProducts>>, TError, TData>, }
): UseQueryOptions<Awaited<ReturnType<typeof getApiProducts>>, TError, TData> & { queryKey: QueryKey } => {
const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getGetApiProductsQueryKey(params);

  
  
    const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiProducts>>> = ({ signal }) => getApiProducts(params, signal);
    
      
      
   return  { queryKey, queryFn, ...queryOptions}}

export type GetApiProductsQueryResult = NonNullable<Awaited<ReturnType<typeof getApiProducts>>>
export type GetApiProductsQueryError = ErrorType<void>

/**
 * @summary Get a list of all products or only store products depending on permissions
 */
export const useGetApiProducts = <TData = Awaited<ReturnType<typeof getApiProducts>>, TError = ErrorType<void>>(
 params?: GetApiProductsParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiProducts>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const queryOptions = getGetApiProductsQueryOptions(params,options)

  const query = useQuery(queryOptions) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryOptions.queryKey ;

  return query;
}

/**
 * Requires verified email; Requires one of the following permissions: Shop.CreateProduct; Requires the following features to be enabled: Shop
 * @summary Create a new product
 */
export const postApiProducts = (
    shopCreateProductRequestBody: BodyType<ShopCreateProductRequestBody>,
 ) => {
      return useCustomClient<ShopCreateProductResponse>(
      {url: `/Api/Products`, method: 'post',
      headers: {'Content-Type': 'application/json', },
      data: shopCreateProductRequestBody
    },
      );
    }
  


export const getPostApiProductsMutationOptions = <TError = ErrorType<void>,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof postApiProducts>>, TError,{data: BodyType<ShopCreateProductRequestBody>}, TContext>, }
): UseMutationOptions<Awaited<ReturnType<typeof postApiProducts>>, TError,{data: BodyType<ShopCreateProductRequestBody>}, TContext> => {
 const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof postApiProducts>>, {data: BodyType<ShopCreateProductRequestBody>}> = (props) => {
          const {data} = props ?? {};

          return  postApiProducts(data,)
        }

        

 
   return  { mutationFn, ...mutationOptions }}

    export type PostApiProductsMutationResult = NonNullable<Awaited<ReturnType<typeof postApiProducts>>>
    export type PostApiProductsMutationBody = BodyType<ShopCreateProductRequestBody>
    export type PostApiProductsMutationError = ErrorType<void>

    /**
 * @summary Create a new product
 */
export const usePostApiProducts = <TError = ErrorType<void>,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof postApiProducts>>, TError,{data: BodyType<ShopCreateProductRequestBody>}, TContext>, }
) => {
    
      const mutationOptions = getPostApiProductsMutationOptions(options);
     
      return useMutation(mutationOptions);
    }
    /**
 * Requires verified email; Requires one of the following permissions: Shop.ViewProduct, Shop.ViewStoreProduct; Requires the following features to be enabled: Shop
 * @summary Get information about a product
 */
export const getApiProductsId = (
    id: number,
 signal?: AbortSignal
) => {
      return useCustomClient<ShopViewProductResponse>(
      {url: `/Api/Products/${id}`, method: 'get', signal
    },
      );
    }
  

export const getGetApiProductsIdQueryKey = (id: number,) => [`/Api/Products/${id}`] as const;
  

    
export const getGetApiProductsIdQueryOptions = <TData = Awaited<ReturnType<typeof getApiProductsId>>, TError = ErrorType<void | ProblemDetails>>(id: number, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiProductsId>>, TError, TData>, }
): UseQueryOptions<Awaited<ReturnType<typeof getApiProductsId>>, TError, TData> & { queryKey: QueryKey } => {
const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getGetApiProductsIdQueryKey(id);

  
  
    const queryFn: QueryFunction<Awaited<ReturnType<typeof getApiProductsId>>> = ({ signal }) => getApiProductsId(id, signal);
    
      
      
   return  { queryKey, queryFn, enabled: !!(id), ...queryOptions}}

export type GetApiProductsIdQueryResult = NonNullable<Awaited<ReturnType<typeof getApiProductsId>>>
export type GetApiProductsIdQueryError = ErrorType<void | ProblemDetails>

/**
 * @summary Get information about a product
 */
export const useGetApiProductsId = <TData = Awaited<ReturnType<typeof getApiProductsId>>, TError = ErrorType<void | ProblemDetails>>(
 id: number, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof getApiProductsId>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const queryOptions = getGetApiProductsIdQueryOptions(id,options)

  const query = useQuery(queryOptions) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryOptions.queryKey ;

  return query;
}

/**
 * Requires verified email; Requires one of the following permissions: Shop.UpdateProduct; Requires the following features to be enabled: Shop
 * @summary Update a product
 */
export const patchApiProductsId = (
    id: number,
    shopUpdateProductRequestBody: BodyType<ShopUpdateProductRequestBody>,
 ) => {
      return useCustomClient<void>(
      {url: `/Api/Products/${id}`, method: 'patch',
      headers: {'Content-Type': 'application/json', },
      data: shopUpdateProductRequestBody
    },
      );
    }
  


export const getPatchApiProductsIdMutationOptions = <TError = ErrorType<ProblemDetails>,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof patchApiProductsId>>, TError,{id: number;data: BodyType<ShopUpdateProductRequestBody>}, TContext>, }
): UseMutationOptions<Awaited<ReturnType<typeof patchApiProductsId>>, TError,{id: number;data: BodyType<ShopUpdateProductRequestBody>}, TContext> => {
 const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof patchApiProductsId>>, {id: number;data: BodyType<ShopUpdateProductRequestBody>}> = (props) => {
          const {id,data} = props ?? {};

          return  patchApiProductsId(id,data,)
        }

        

 
   return  { mutationFn, ...mutationOptions }}

    export type PatchApiProductsIdMutationResult = NonNullable<Awaited<ReturnType<typeof patchApiProductsId>>>
    export type PatchApiProductsIdMutationBody = BodyType<ShopUpdateProductRequestBody>
    export type PatchApiProductsIdMutationError = ErrorType<ProblemDetails>

    /**
 * @summary Update a product
 */
export const usePatchApiProductsId = <TError = ErrorType<ProblemDetails>,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof patchApiProductsId>>, TError,{id: number;data: BodyType<ShopUpdateProductRequestBody>}, TContext>, }
) => {
    
      const mutationOptions = getPatchApiProductsIdMutationOptions(options);
     
      return useMutation(mutationOptions);
    }
    /**
 * Requires verified email; Requires one of the following permissions: Shop.DeleteProduct; Requires the following features to be enabled: Shop
 * @summary Delete a product
 */
export const deleteApiProductsId = (
    id: number,
 ) => {
      return useCustomClient<void>(
      {url: `/Api/Products/${id}`, method: 'delete'
    },
      );
    }
  


export const getDeleteApiProductsIdMutationOptions = <TError = ErrorType<ProblemDetails>,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof deleteApiProductsId>>, TError,{id: number}, TContext>, }
): UseMutationOptions<Awaited<ReturnType<typeof deleteApiProductsId>>, TError,{id: number}, TContext> => {
 const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof deleteApiProductsId>>, {id: number}> = (props) => {
          const {id} = props ?? {};

          return  deleteApiProductsId(id,)
        }

        

 
   return  { mutationFn, ...mutationOptions }}

    export type DeleteApiProductsIdMutationResult = NonNullable<Awaited<ReturnType<typeof deleteApiProductsId>>>
    
    export type DeleteApiProductsIdMutationError = ErrorType<ProblemDetails>

    /**
 * @summary Delete a product
 */
export const useDeleteApiProductsId = <TError = ErrorType<ProblemDetails>,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof deleteApiProductsId>>, TError,{id: number}, TContext>, }
) => {
    
      const mutationOptions = getDeleteApiProductsIdMutationOptions(options);
     
      return useMutation(mutationOptions);
    }
    /**
 * Requires verified email; Requires one of the following permissions: Shop.BuyProduct; Requires the following features to be enabled: Shop
 * @summary Buy a product
 */
export const postApiProductsBuyId = (
    id: number,
 ) => {
      return useCustomClient<void>(
      {url: `/Api/Products/Buy/${id}`, method: 'post'
    },
      );
    }
  


export const getPostApiProductsBuyIdMutationOptions = <TError = ErrorType<ProblemDetails>,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof postApiProductsBuyId>>, TError,{id: number}, TContext>, }
): UseMutationOptions<Awaited<ReturnType<typeof postApiProductsBuyId>>, TError,{id: number}, TContext> => {
 const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof postApiProductsBuyId>>, {id: number}> = (props) => {
          const {id} = props ?? {};

          return  postApiProductsBuyId(id,)
        }

        

 
   return  { mutationFn, ...mutationOptions }}

    export type PostApiProductsBuyIdMutationResult = NonNullable<Awaited<ReturnType<typeof postApiProductsBuyId>>>
    
    export type PostApiProductsBuyIdMutationError = ErrorType<ProblemDetails>

    /**
 * @summary Buy a product
 */
export const usePostApiProductsBuyId = <TError = ErrorType<ProblemDetails>,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof postApiProductsBuyId>>, TError,{id: number}, TContext>, }
) => {
    
      const mutationOptions = getPostApiProductsBuyIdMutationOptions(options);
     
      return useMutation(mutationOptions);
    }
    