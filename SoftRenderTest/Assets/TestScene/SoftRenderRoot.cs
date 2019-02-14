using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoftRender {

public class Root {

	public Camera	pers_camera;

	public Camera	ortho_camera;

	public Matrix4x4	view_mat_ortho;
	public Matrix4x4	view_mat_pers;
	public Matrix4x4	proj_mat_ortho;
	public Matrix4x4	proj_mat_pers;

	public Matrix4x4	proj_mat_hybrid;

	// ================================================================ //

	public void		setCameras(Camera pers_camera, Camera ortho_camera)
	{
		this.pers_camera = pers_camera;
		this.ortho_camera = ortho_camera;

		this.view_mat_ortho = this.ortho_camera.worldToCameraMatrix;
		this.view_mat_pers  = this.pers_camera.worldToCameraMatrix;
		this.proj_mat_ortho = this.ortho_camera.projectionMatrix;
		this.proj_mat_pers  = this.pers_camera.projectionMatrix;

		this.proj_mat_ortho = SoftRender.Root.convertProjectionMatrix(this.proj_mat_ortho);
		this.proj_mat_pers  = SoftRender.Root.convertProjectionMatrix(this.proj_mat_pers);

		this.proj_mat_hybrid = this.proj_mat_pers;
	}

	public Vector3	transformPosition3Dto2D(Vector3 position_3d)
	{
		Vector3		position_2d = position_3d;

		return(position_2d);
	}

	// ================================================================ //

	public static Matrix4x4		convertProjectionMatrix(Matrix4x4 mat)
	{
		if(mat.m33 == 0.0f) {

			mat.m11 = -mat.m11;
			mat.m22 = -(mat.m22 + 1.0f)/2.0f;
			mat.m23 = -mat.m23/2.0f;

		} else {

			mat.m11 = -mat.m11;
			mat.m22 = -mat.m22/2.0f;
			mat.m23 = -(mat.m23 - 1.0f)/2.0f;
		}

		return(mat);
	}

	public static Matrix4x4	CreateProjectionMatrixPerspective(float fovy, float width_over_height, float znear, float zfar)
	{
		Matrix4x4	matrix = Matrix4x4.identity;

		float		width, height;

		height = Mathf.Tan(Mathf.Deg2Rad*fovy/2.0f);

		width = height*width_over_height;

		matrix.m00 = 1.0f/width;
		matrix.m10 = 0.0f;
		matrix.m20 = 0.0f;
		matrix.m30 = 0.0f;

		matrix.m01 = 0.0f;
		matrix.m11 = 1.0f/height;
matrix.m11 = -1.0f/height;
		matrix.m21 = 0.0f;
		matrix.m31 = 0.0f;

		matrix.m02 = 0.0f;
		matrix.m12 = 0.0f;
		matrix.m22 = -zfar/(zfar - znear);
matrix.m22 = znear/(zfar - znear);
		matrix.m32 = -1.0f;
		//matrix.m22 = -2.0f/(zfar - znear);
		//matrix.m32 =  1.0f;

		matrix.m03 =  0.0f;
		matrix.m13 =  0.0f;
		matrix.m23 = -zfar*znear/(zfar - znear);
matrix.m23 = zfar*znear/(zfar - znear);
		matrix.m33 =  0.0f;
		//matrix.m23 = -(zfar + znear)/(zfar - znear);
		//matrix.m33 =  0.0f;

		return(matrix);
	}

} // public class Root {

} // namespace SoftRender {
