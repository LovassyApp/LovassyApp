import { AuthenticatedGuard } from "../../core/routing/guards/authenticatedGuard";
import { EmailVerifiedGuard } from "../../core/routing/guards/emailVerifiedGuard";
import { FeatureGuard } from "../../core/routing/guards/featureGuard";
import { PermissionGuard } from "../../core/routing/guards/permissionGuard";
import { Route } from "react-router-dom";
import { lazy } from "react";

export const getImageVotingsRoutes = () => {
    const AuthenticatedLayout = lazy(() => import("../../core/routing/layouts/authenticatedLayout"));

    const ManageImageVotingsPage = lazy(() => import("./pages/manageImageVotingsPage"));
    const ImageVotingsPage = lazy(() => import("./pages/imageVotingsPage"));
    const ImageVotingPage = lazy(() => import("./pages/imageVotingPage"));

    return (
        <>
            <Route element={<AuthenticatedGuard />}>
                <Route element={<EmailVerifiedGuard />}>
                    <Route element={<AuthenticatedLayout />}>
                        <Route element={<FeatureGuard features={["ImageVotings"]} />}>
                            <Route element={<PermissionGuard permissions={["ImageVotings.IndexImageVotings"]} />}>
                                <Route path="/image-votings/manage" element={<ManageImageVotingsPage />} />
                            </Route>
                            <Route
                                element={
                                    <PermissionGuard
                                        permissions={[
                                            "ImageVotings.IndexImageVotings",
                                            "ImageVotings.IndexActiveImageVotings",
                                        ]}
                                    />
                                }
                            >
                                <Route path="/image-votings" element={<ImageVotingsPage />} />
                            </Route>
                            <Route
                                element={
                                    <PermissionGuard
                                        permissions={[
                                            "ImageVotings.ViewImageVoting",
                                            "ImageVotings.ViewActiveImageVoting",
                                        ]}
                                    />
                                }
                            >
                                <Route path="/image-votings/:id" element={<ImageVotingPage />} />
                            </Route>
                        </Route>
                    </Route>
                </Route>
            </Route>
        </>
    );
};
