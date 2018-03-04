namespace Parking.BL
{
	public interface IModelBuilder<out TEntity>
	{
		TEntity Build();
	}
	public interface IModelBuilder<in TModel, out TEntity>
	{
		TEntity Build(TModel param);
	}
}