import React from 'react'

export const GithubLink: React.FC<{githubLink: string}> = ({ githubLink }) => <a
	role="button"
	className="bp3-button bp3-minimal bp3-icon-git-repo"
	href={githubLink}
>
            GitHub
</a>
