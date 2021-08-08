import React from 'react'

export const WikiImage: React.FC<{imageUrl: string}> = ({ imageUrl }) => <div className="hide">
	<img src={imageUrl} width="80%" alt=""/>
</div>
