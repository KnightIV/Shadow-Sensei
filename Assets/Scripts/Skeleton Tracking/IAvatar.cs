public interface IAvatar {

    ISkeletonProvider SkeletonProvider { get; }

    void SwapSkeletonProvider(ISkeletonProvider newProvider);
}
