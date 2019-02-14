using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// .
public class TextureBuilder {

	public Texture2D		texture;
	public Color[]			pixels;

	public int				width;
	public int				height;
	public TextureFormat	format;

	public class Pen {

		public TextureBuilder	builder;
		public Color	color = Color.white;
		public virtual Color	get_color(int x, int y)
		{
			return(this.color);
		}
	}

	public Pen	solid_pen = new Pen();

	// ================================================================ //

	public TextureBuilder()
	{
	}

	// ================================================================ //

	public void		resize(int division)
	{
		int		new_width = this.width/division;
		int		new_height = this.height/division;

		Color[]		new_color = new Color[new_width*new_height];

		for(int y = 0;y < new_height;y++) {
			for(int x = 0;x < new_width;x++) {

				Color	color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

				for(int sx = 0;sx < division;sx++) {
					for(int sy = 0;sy < division;sy++) {

						color += this.get(x*division + sx, y*division + sy);
					}
				}

				if(color.a > 0.0f) {
					color /= color.a;
				}

				new_color[y*new_width + x] = color;
			}
		}

		this.width = new_width;
		this.height = new_height;
		this.pixels = new_color;
	}

	public void		createPlain(int width, int height, TextureFormat format)
	{
		this.width  = width;
		this.height = height;
		this.format = format;

		this.texture = new Texture2D(this.width, this.height, this.format, false);

		this.pixels = new Color[this.width*this.height];

		for(int i = 0;i < this.pixels.Length;i++) {

			this.pixels[i] = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}

		this.finalize();
	}

	public void		create(Texture2D texture)
	{
		this.width  = texture.width;
		this.height = texture.height;
		this.format = texture.format;

		this.texture = texture;

		this.pixels = this.texture.GetPixels();
	}

	public void		finalize()
	{
		this.texture.SetPixels(this.pixels);
		this.texture.Apply();
	}

	// ================================================================ //

	public void		clear(Color color)
	{
		for(int i = 0;i < this.pixels.Length;i++) {

			this.pixels[i] = color;
		}
	}

	public void		drawLine(int x0, int y0, int x1, int y1, Color color)
	{
		this.solid_pen.color = color;

		this.drawLine(x0, y0, x1, y1, this.solid_pen);
	}
	public void		drawLine(int x0, int y0, int x1, int y1, Pen pen)
	{
		if(x0 == x1 && y0 == y1) {
			this.put(x0, y0, pen);
		} else {
			this.draw_line(x0, y0, x1, y1, pen);
		}
	}

	protected void	draw_line(int x0, int y0, int x1, int y1, Pen pen)
	{
		float		dx = x1 - x0;
		float		dy = y1 - y0;

		int		x, y;

		if(Mathf.Abs(dx) >= Mathf.Abs(dy)) {

			int		udx = 1;

			if(x1 - x0 < 0) {
				udx = -1;
			}

			for(x = x0;x != x1 + udx;x += udx) {

				y = Mathf.RoundToInt(((float)(x - x0))*dy/dx + y0);

				this.put(x, y, pen);
			}

		} else {

			int		udy = 1;

			if(y1 - y0 < 0) {
				udy = -1;
			}

			for(y = y0;y != y1 + udy;y += udy) {

				x = Mathf.RoundToInt(((float)(y - y0))*dx/dy + x0);

				this.put(x, y, pen);
			}
		}
	}
	public void		drawLine(Vector3 p0, Vector3 p1, Color color)
	{
		this.drawLine(Mathf.RoundToInt(p0.x), Mathf.RoundToInt(p0.y), Mathf.RoundToInt(p1.x), Mathf.RoundToInt(p1.y), Color.white);
	}

	// ---------------------------------------------------------------- //

	public void		drawTriangle(int x0, int y0, int x1, int y1, int x2, int y2, Color color)
	{
		this.solid_pen.color = color;

		this.drawTriangle(x0, y0, x1, y1, x2, y2, this.solid_pen);
	}
	public void		drawTriangle(int x0, int y0, int x1, int y1, int x2, int y2, Pen pen)
	{
		this.drawLine(x0, y0, x1, y1, pen);
		this.drawLine(x1, y1, x2, y2, pen);
		this.drawLine(x2, y2, x0, y0, pen);
	}

	public void		fillTriangle(int x0, int y0, int x1, int y1, int x2, int y2, Color color)
	{
		this.solid_pen.color = color;

		this.fillTriangle(x0, y0, x1, y1, x2, y2, this.solid_pen);
	}
	public void		fillTriangle(int x0, int y0, int x1, int y1, int x2, int y2, Pen pen)
	{

		if(y0 == y1 && y1 == y2) {

			int		xl, xr;

			xl = Mathf.Min(x0, x1, x2);
			xr = Mathf.Max(x0, x1, x2);

			this.draw_line(xl, y0, xr, y0, pen);

		} else {

			int		tx0 = x0, ty0 = y0;
			int		tx1 = x1, ty1 = y1;
			int		tx2 = x2, ty2 = y2;

			if(y1 > y0) {
				if(y2 > y1) {
					// 2-1-0
					tx0 = x0;ty0 = y0;
					tx1 = x1;ty1 = y1;
					tx2 = x2;ty2 = y2;
				} else {

					if(y2 > y0) {
						// 1-2-0
						tx0 = x0;ty0 = y0;
						tx1 = x2;ty1 = y2;
						tx2 = x1;ty2 = y1;
					} else {
						// 1-0-2
						tx0 = x2;ty0 = y2;
						tx1 = x0;ty1 = y0;
						tx2 = x1;ty2 = y1;
					}
				}
			} else { // y0 > y1

				if(y2 > y0) {
					// 2-0-1
					tx0 = x1;ty0 = y1;
					tx1 = x0;ty1 = y0;
					tx2 = x2;ty2 = y2;
				} else {
					if(y2 > y1) {
						// 0-2-1
						tx0 = x1;ty0 = y1;
						tx1 = x2;ty1 = y2;
						tx2 = x0;ty2 = y0;
					} else {
						// 0-1-2
						tx0 = x2;ty0 = y2;
						tx1 = x1;ty1 = y1;
						tx2 = x0;ty2 = y0;
					}
				}
			}

			this.fill_triangle(tx0, ty0, tx1, ty1, tx2, ty2, pen);
		}
	}

	protected void		fill_triangle(int x0, int y0, int x1, int y1, int x2, int y2, Pen pen)
	{
		int		y;
		float	lx, rx;
		int		ilx, irx;

		if(y1 != y0 && y2 != y0) {

			float		dlx = x1 - x0;
			float		dly = y1 - y0;
			float		drx = x2 - x0;
			float		dry = y2 - y0;

			int		udy = 1;

			if(y1 - y0 < 0) {
				udy = -1;
			}

			for(y = y0;y != y1 + udy;y += udy) {

				lx = ((float)(y - y0))*dlx/dly + x0;
				rx = ((float)(y - y0))*drx/dry + x0;

				if(rx < lx) {
					MathUtility.swap(ref lx, ref rx);
				}

				ilx = Mathf.FloorToInt(lx);
				irx = Mathf.CeilToInt(rx);

				this.drawLine(ilx, y, irx, y, pen);
			}
		}


		if(y2 != y1 && y2 != y0) {

			float	dlx = x2 - x1;
			float	dly = y2 - y1;
			float	drx = x2 - x0;
			float	dry = y2 - y0;

			int		udy = 1;

			if(y2 - y1 < 0) {
				udy = -1;
			}

			for(y = y1;y != y2 + udy;y += udy) {

				lx = ((float)(y - y1))*dlx/dly + x1;
				rx = ((float)(y - y0))*drx/dry + x0;

				if(rx < lx) {
					MathUtility.swap(ref lx, ref rx);
				}

				ilx = Mathf.FloorToInt(lx);
				irx = Mathf.CeilToInt(rx);

				this.drawLine(ilx, y, irx, y, pen);
			}
		}
	}

	// ---------------------------------------------------------------- //

	public void		put(int x, int y, Color color)
	{
		this.solid_pen.color = color;

		this.put(x, y, this.solid_pen);
	}
	public void		put(int x, int y, Pen pen)
	{
		x = Mathf.Clamp(x, 0, this.width - 1);
		y = Mathf.Clamp(y, 0, this.height - 1);

		this.pixels[y*this.width + x] = pen.get_color(x, y);
	}

	public Color	get(int x, int y)
	{
		x = Mathf.Clamp(x, 0, this.width - 1);
		y = Mathf.Clamp(y, 0, this.height - 1);

		return(this.pixels[y*this.width + x]);
	}

	// ================================================================ //



};


// ======================================================================== //
//																			//
// ======================================================================== //

