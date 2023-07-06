import { Outlet, RootRoute, Router } from "@tanstack/router";

import { FullScreenLoading } from "../components/fullScreenLoading";
import { authRoutes } from "../../features/auth/authRoutes";
import { lazy } from "react";

const TanStackRouterDevtools =
  import.meta.env.PROD
      ? () => null
      : lazy(() =>
          import("@tanstack/router-devtools").then((res) => ({
              default: res.TanStackRouterDevtools,
          })),
      );

export const rootRoute = new RootRoute({
    component: () => (
        <>
            <TanStackRouterDevtools />
            <Outlet />
        </>
    ),
});

const routeTree = rootRoute.addChildren([
    ...authRoutes,
]);

export const appRouter = new Router({
    routeTree,
    defaultPendingComponent: () => (
        <FullScreenLoading />
    )
});

declare module "@tanstack/router" {
    interface Register {
      router: typeof appRouter
    }
  }
