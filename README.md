컵헤드 2D 모작 
수정해야 할 사항들
1. 여러번 대쉬 할 시 미끄러지는 현상
2. 패링 중 TimeScale 줄여주는 작동을 player 연관된 스크립트에서 실행해주기
   ( gameObject를 파괴하게 되면 IParry를 상속한 오브젝트가 Destroy 되기 때문에 이런 식으로 작동하게 하면
   안된다. )
3. Boss와 Player의 이펙트 및 사운드 추가해주기
4. Player의 BulletCollider 위치 등 상황에 따라 위치가 고정되어 있는 Object들은 미리 캐싱해서 실행해주기(최적화)
5. 
