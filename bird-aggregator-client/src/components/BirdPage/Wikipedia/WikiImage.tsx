import React from "react"

export const WikiImage: React.FC<{imageUrl: string}> = ({imageUrl}) => {
    return <div className="hide">
				<img src={imageUrl} width="80%"/>
			</div>

}